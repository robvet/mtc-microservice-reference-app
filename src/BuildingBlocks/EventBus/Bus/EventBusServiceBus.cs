using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Connection;
using EventBus.Events;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventBus.EventBus
{
    public class EventBusServiceBus : IEventBus
    {
        private readonly Dictionary<MessageEventEnum, Type> _handlers = new Dictionary<MessageEventEnum, Type>();
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
        private readonly SubscriptionClient _subscriptionClient;
        private ILogger _logger;

        public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection,
            string subscriptionClientName)
        {
            _serviceBusPersisterConnection = serviceBusPersisterConnection;

            // Here, we register a service with a specific subscription
            _subscriptionClient = new SubscriptionClient(
                serviceBusPersisterConnection.ServiceBusConnectionStringBuilder,
                subscriptionClientName);

            // Register the callback method that will be invoked a message of interest is received
            RegisterSubscriptionClientMessageHandler();
        }

        public IServiceProvider ServiceProvider { get; set; }
        public string CustomName { get; set; }

        /// <summary>
        /// Publish a mesage to Azure Service Bus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload">The message</param>
        /// <param name="eventEnum">Enum representing message type</param>
        /// <param name="correlationToken">Unique value passed with request for tracking</param>
        /// <returns></returns>
        public async Task Publish<T>(T payload, MessageEventEnum eventEnum, string correlationToken)
        {
            try
            {
                var jsonMessage = JsonConvert.SerializeObject(payload);

                var message = new Message
                {
                    MessageId = new Guid().ToString(),
                    Body = Encoding.UTF8.GetBytes(jsonMessage),
                    UserProperties =
                    {
                        {"Event", eventEnum.ToString()},
                        {"correlationToken", correlationToken}
                    }
                };

                var topicClient = _serviceBusPersisterConnection.CreateModel();
                await topicClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                _logger = ServiceProvider.GetService<ILogger<EventBusServiceBus>>();
                _logger.LogError(new EventId(ex.HResult),
                    ex,
                    "Exception throw in Event.Publish for {eventType} : {message}", eventEnum, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Regsiter Service Event Type
        /// </summary>
        /// <param name="eventName">Enum Representation of Event</param>
        /// <param name="eventType">Type of the underlying Event</param>
        public void Subscribe(MessageEventEnum eventName, Type eventType)
        {
            // Ensure that eventType implements IIntegratedEventHandler
            if (!typeof(IIntegratedEventHandler).IsAssignableFrom(eventType))
                throw new ArgumentException($"{eventType} is not valid agrument for EventBus.Subscribe");

            // Make sure that the IServiceProvider property has been set
            if (ServiceProvider == null)
                throw new MissingFieldException("IServiceProvider must be set in EventBus");

            _handlers.Add(eventName, eventType);
        }

        /// <summary>
        /// Register call back for message from Azure Service Bus.
        /// This method will be invoked when a message of interest is added to the EventBus
        /// </summary>
        private void RegisterSubscriptionClientMessageHandler()
        {
            _subscriptionClient.RegisterMessageHandler(
                // Regsiters the call backback for each event
                async (message, token) =>
                {
                    // Get the event type
                    if (message.UserProperties.TryGetValue("Event", out var eventType))
                    {
                        // Track the delivery count
                        var deliveryCount = message.SystemProperties.DeliveryCount;

                        // Covert event name Enum to string
                        if (Enum.TryParse<MessageEventEnum>(eventType.ToString(), out var eventName))
                        {
                            // if _handlers.Count is empty, then just ignore and exit eventHandler. 
                            if (_handlers.Count < 1)
                            {
                                await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                                return;
                            }

                            if (_handlers.All(x => x.Key != eventName))
                            {
                                throw new MissingMemberException($"{eventName} is not registered!");
                            }

                            // Get concrete type for eventname
                            var concreteType = _handlers[eventName];

                            // Call DI containers and get all registered eventhandlers
                            var registeredEventHandlers = ServiceProvider.GetServices<IIntegratedEventHandler>();

                            IIntegratedEventHandler targetType = null;

                            // Itereate registered event handlers 
                            foreach (var service in registeredEventHandlers)
                                // Find the registered event handler that matches the currently-raised event
                                if (service.GetType() == concreteType)
                                {
                                    targetType = service;
                                    break;
                                }

                            // Ensure eventhandler is valid
                            if (targetType == null)
                            {
                                throw new MissingMemberException($"{eventName} is not registered!");
                            }

                             // Invoke the currently-raised event
                            await targetType.HandleAsync(message);

                            _logger = ServiceProvider.GetService<ILogger<EventBusServiceBus>>();
                            _logger.LogInformation("Event handled: {eventName}", eventName);
                            
                            // Complete the message so that it is not received again.
                            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                        }
                    }
                },
                // We have hard-coded configuration values. Make dynamic.
                new MessageHandlerOptions(ExceptionReceivedHandler) {MaxConcurrentCalls = 1, AutoComplete = false});
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var errorMessage = new StringBuilder();
            errorMessage.Append($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");

            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            errorMessage.Append("Exception context for troubleshooting:");
            errorMessage.Append($"- Endpoint: {context.Endpoint}");
            errorMessage.Append($"- Entity Path: {context.EntityPath}");
            errorMessage.Append($"- Executing Action: {context.Action}");

            _logger = ServiceProvider.GetService<ILogger<EventBusServiceBus>>();
            _logger.LogError(new EventId(exceptionReceivedEventArgs.Exception.HResult),
                exceptionReceivedEventArgs.Exception,
                "Exception throw in {message}", errorMessage.ToString());

            return Task.CompletedTask;
        }
    }
}