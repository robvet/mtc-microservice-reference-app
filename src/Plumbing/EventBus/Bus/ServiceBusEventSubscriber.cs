using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Events;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventBus.Bus
{
    public class ServiceBusEventSubscriber : IEventBusSubscriber
    {
        private readonly string _boundServiceName;
        private readonly ILogger<ServiceBusEventSubscriber> _logger;
        private SubscriptionClient _subscriptionClient;
        private string _correlationToken;
        private string _currentMessageType;
        private readonly Dictionary<string, Type> _events = new Dictionary<string, Type>();
        private readonly Dictionary<string, IMessageEventHandler> _eventHandlers = new Dictionary<string, IMessageEventHandler>();

        public ServiceBusEventSubscriber(string connectionString,
            string boundServiceName,
            ILogger<ServiceBusEventSubscriber> logger,
            string subscriptionName
            )
        {
            _logger = logger;
            _boundServiceName = boundServiceName;
            CreateSubscriptionClient(connectionString, subscriptionName, logger);
        }

        // Create SubscriptionClient and enable the message pump
        private void CreateSubscriptionClient(string connectionString, string subscriptionName, ILogger logger)
        {
            try
            {
                var builder = new ServiceBusConnectionStringBuilder(connectionString);

                _subscriptionClient = new SubscriptionClient(builder, subscriptionName);

                //_subscriptionClient = new SubscriptionClient(connectionString, topicName, subscriptionName);
                // Register the callback method that will be invoked a message of interest is received
                RegisterSubscriptionMessageHandler();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot create SubscriptionClient in ServiceBusEventProvider: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Register service events
        /// </summary>
        public void Subscribe<T>(IMessageEventHandler messageHandler)
        {
            _events.Add(typeof(T).Name, typeof(T));
            _eventHandlers.Add(typeof(T).Name, messageHandler);
        }

        /// <summary>
        /// Registers call-back method for Azure ServiceBus message pump
        /// </summary>
        private void RegisterSubscriptionMessageHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // MaxConcurrentCalls specifies number of messages to process concurrently.
                // By default, this is set to 1, which means that we'll process 1 message
                // off the queue, and only when that has finished will we move onto the next one.
                // MaxConcurrentCalls applies to just the process that you are running.
                // If for example you had three instances of the Receiver application each
                // listening to the same queue with MaxConcurrentCalls set to 1, then
                // you would process three messages in parallel.
                MaxConcurrentCalls = 1,
                AutoComplete = false, 
            };

            // Registers callback method - the first argument - with Azure Service Bus.
            _subscriptionClient.RegisterMessageHandler(ProcessMessage, messageHandlerOptions);
        }

        /// <summary>
        /// CallBack method which is invoked for each message
        /// </summary>
        /// <param name="message">The messageEvent, or message</param>
        /// <param name="token">A cancellation token instructing the method to stop</param>
        private async Task ProcessMessage(Message message, CancellationToken token)
        {
            // This code block is the callback that is invoked for each published event.
            // It is invoked for each message the the topic places in the subscription.
            // Start by getting event type which is included as a metadata property for the event
            if (message.UserProperties.TryGetValue("Event", out var eventTypeOut))
            {
                // Get eventType by name
                string @event = eventTypeOut?.ToString() ?? throw new ArgumentNullException($"EventType is null in MessagePump in ServiceBusEventProvider");

                // Set message type for internal logging
                _currentMessageType = @event;

                // Extract Correlation Token for request -- used for internal logging
                message.UserProperties.TryGetValue("_correlationToken", out var correlationToken);

                // Set correlationToken for internal logging
                _correlationToken = correlationToken != null ? correlationToken.ToString() : "Correlation Token Unavailable";

                if (_events.Count < 1)
                {
                    var errorMessage =
                        $"No events have been registered in ProcessMessage for Service {_boundServiceName} for CorrelationToken {_correlationToken}";
                    _logger.LogError(errorMessage);

                    throw new MissingMemberException(errorMessage);
                }
                                
                // Get Event type
                var eventType = _events[@event];

                if (eventType == null)
                {
                    var errorMessage =
                        $"Missing Event for {@event} in ProcessMessage for Service {_boundServiceName} for CorrelationToken {_correlationToken}";
                    _logger.LogError(errorMessage);

                    throw new MissingMemberException(errorMessage);
                }

                MessageEvent eventMessage;

                try
                {
                    var body = Encoding.UTF8.GetString(message.Body);
                    eventMessage = JsonConvert.DeserializeObject(body, eventType) as MessageEvent;
                    eventMessage.MessageId = message.MessageId; //CorrelationId
                }
                catch (JsonException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }


                // Deserialize message here so that the eventHandler has no dependency on Azure Service Bus
                //var body = Encoding.UTF8.GetString(message.Body);
                //var eventMessage = JsonConvert.DeserializeObject(body, eventType) as MessageEvent;

                // Obtain reference to EventHandler
                var eventHandler = _eventHandlers[@event];

                if (eventHandler == null)
                {
                    var errorMessage =
                        $"Missing EventHandler for {@event} in ProcessMessage for Service {_boundServiceName} for CorrelationToken {_correlationToken}";
                    _logger.LogError(errorMessage);

                    throw new MissingMemberException(errorMessage);
                }

                _logger.LogInformation(
                    $"Processing event {@event} for Service {_boundServiceName} for CorrelationToken {_correlationToken}");

                // Invoke EventHandler
                await eventHandler.HandleAsync(eventMessage);

                _logger.LogInformation(
                    $"Processed event {@event} for Service {_boundServiceName} for CorrelationToken {_correlationToken}");

                // Complete the message so that it is not received again.
                await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var errorMessage = new StringBuilder();
            errorMessage.Append($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");

            //For troubleshooting...
            //var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            //errorMessage.Append("Exception context for troubleshooting:");
            //errorMessage.Append($"- Endpoint: {context.Endpoint}");
            //errorMessage.Append($"- Entity Path: {context.EntityPath}");
            //errorMessage.Append($"- Executing Action: {context.Action}");

            //_logger.LogError(new EventId(exceptionReceivedEventArgs.Exception.HResult),
            //    exceptionReceivedEventArgs.Exception,
            //    "Exception throw in ServiceBusEventProvider-MessageHandler for {eventName} for Bound Service {BoundService} and Correlation Token {_correlationToken}", errorMessage.ToString());

            errorMessage.Append($"Exception thrown in EventBus Subscriber - Event {_currentMessageType} for Service {_boundServiceName} and Correlation Token {_correlationToken}; Error message = {errorMessage}");
            _logger.LogError(errorMessage.ToString());

            return Task.CompletedTask;
        }
    }
}
