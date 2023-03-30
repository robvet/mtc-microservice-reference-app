using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandBus.Commands;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommandBus.Bus
{
    public class ServiceBusCommandProvider : ICommandBus
    {
        private QueueClient _queueClient;
        private readonly string _queueName;
        
        private readonly string _boundServiceName;
        private readonly ILogger<ServiceBusCommandProvider> _logger;
        private string _correlationToken;
        private string _currentCommandType;
        private readonly Dictionary<string, Type> _commands =
            new Dictionary<string, Type>();
        private readonly Dictionary<string, ICommandHandler> _commandHandlers =
            new Dictionary<string, ICommandHandler>();

        public ServiceBusCommandProvider(string connectionString,
            string boundServiceName,
            string queueName,
            ILogger<ServiceBusCommandProvider> logger,
            bool consumeMessages)
        {
            _logger = logger;
            _queueName = queueName;
            _boundServiceName = boundServiceName;
            
            CreateClientSendMessages(connectionString);

            if (consumeMessages)
            {
                CreateClientConsumeMessages(connectionString);
            }
        }

        // Create QueueClient to send messages
        private void CreateClientSendMessages(string connectionString)
        {
            try
            {
                _queueClient = new QueueClient(connectionString, _queueName); ;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot create QueueClientSendMessages in ServiceBusProvider: {ex.Message}");
                throw;
            }
        }

        // Create QueueClient to consume messages
        private void CreateClientConsumeMessages(string connectionString)
        {
            try
            {
                _queueClient = new QueueClient(connectionString, _queueName);

                // Register the callback method that will be invoked a message of interest is received
                RegisterSubscriptionMessageHandler();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot create QueueClientConsumeMessages in ServiceBusProvider: {ex.Message}");
                throw;
            }
        }

        public Task<Command> Consume()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Register command consumer
        /// </summary>
        public void Subscribe<T>(ICommandHandler commandHandler)
        {
            _commands.Add(typeof(T).Name, typeof(T));
            _commandHandlers.Add(typeof(T).Name, commandHandler);
        }

        /// <summary>
        /// Publish message to Azure queue
        /// </summary>
        public async Task Send<T>(Command command)
        {
            try
            {
                var jsonMessage = JsonConvert.SerializeObject(command);

                var currentCommand = new Message
                {
                    MessageId = new Guid().ToString(),
                    Body = Encoding.UTF8.GetBytes(jsonMessage),
                    UserProperties =
                    {
                        {"Command", typeof(T).Name},
                        {"_correlationToken", command.CorrelationToken}
                    }
                };

                _logger.LogInformation(
                    $"Publishing {typeof(T).Name} with CorrelationToken {command.CorrelationToken} for Service {_boundServiceName}");

                await _queueClient.SendAsync(currentCommand);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(ex.HResult),
                    ex,
                    $"Exception throw in Publish for Service {_boundServiceName} for Event {typeof(T).Name} for CorrelationToken {command.CorrelationToken} : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Registers call-back method for Azure ServiceBus message pump
        /// </summary>
        private void RegisterSubscriptionMessageHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            // Registers callback method - the first argument - with Azure Service Bus.
            _queueClient.RegisterMessageHandler(ProcessMessage, messageHandlerOptions);
        }

        /// <summary>
        /// CallBack method which will be invoked for each message
        /// </summary>
        /// <param name="message">The messageEvent, or message</param>
        /// <param name="token">A cancellation token instructing the method to stop</param>
        private async Task ProcessMessage(Message message, CancellationToken token)
        {
            // This code block is the callback that is invoked for each published event.
            // It is invoked for each message placed in the subscription.
            // Start by getting event type which is included as a metadata property for the event
            if (message.UserProperties.TryGetValue("Command", out var eventTypeOut))
            {
                // Get eventType by name
                string @command = eventTypeOut?.ToString() ?? throw new ArgumentNullException($"Command is null in MessagePump in ServiceBusProvider");

                // Set message type for internal logging
                _currentCommandType = @command;

                // Extract Correlation Token for request -- used for internal logging
                message.UserProperties.TryGetValue("_correlationToken", out var correlationToken);

                // Set correlationToken for internal logging
                _correlationToken = correlationToken != null ? correlationToken.ToString() : "Correlation Token Unavailable";

                // Deserialize EventMessage
                var commandType = _commands[@command];

                if (commandType == null)
                {
                    var errorMessage =
                        $"Missing Command for {@command} in ProcessMessage for Service {_boundServiceName} for CorrelationToken {_correlationToken}";
                    _logger.LogError(errorMessage);

                    throw new MissingMemberException(errorMessage);
                }

                // Deserailize message here so that the eventHandler has no dependency on Azure Service Bus
                var body = Encoding.UTF8.GetString(message.Body);
                var commandMessage = JsonConvert.DeserializeObject(body, commandType) as Command;

                // Obtain reference to EventHandler
                var commandHandler = _commandHandlers[@command];

                if (commandHandler == null)
                {
                    var errorMessage =
                        $"Missing EventHandler for {@command} in ProcessMessage for Service {_boundServiceName} for CorrelationToken {_correlationToken}";
                    _logger.LogError(errorMessage);

                    throw new MissingMemberException(errorMessage);
                }

                _logger.LogInformation(
                    $"Processing event {@command} for Service {_boundServiceName} for CorrelationToken {_correlationToken}");

                // Invoke EventHandler
                await commandHandler.HandleAsync(commandMessage);

                _logger.LogInformation(
                    $"Processed event {@command} for Service {_boundServiceName} for CorrelationToken {_correlationToken}");

                ////TODO: Ensure that event type and event message is valid for this subscription

                ////var commandHandler = JsonConvert.DeserializeObject(body, commandHandlerType) as ICommandHandler;

                //if (commandHandler == null)
                //{
                //    var errorMessage =
                //        $"Missing EventHandler for {@command} in ProcessMessage for Service {_boundServiceName} for CorrelationToken {_correlationToken}";
                //    _logger.LogError(errorMessage);

                //    throw new MissingMemberException(errorMessage);
                //}

                //_logger.LogInformation(
                //    $"Processing event {@command} for Service {_boundServiceName} for CorrelationToken {_correlationToken}");
                
                //// Invoke EventHandler
                //await commandHandler.HandleAsync(commandMessage);

                //_logger.LogInformation(
                //    $"Processed event {@command} for Service {_boundServiceName} for CorrelationToken {_correlationToken}");

                // Complete the message so that it is not received again.
                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
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
            //    "Exception throw in ServiceBusProvider-MessageHandler for {eventName} for Bound Service {BoundService} and Correlation Token {_correlationToken}", errorMessage.ToString());

            errorMessage.Append($"Exception throw in ServiceBusProvider-MessageHandler for Event {_currentCommandType} for Service {_boundServiceName} and Correlation Token {_correlationToken}; Error message = {errorMessage}");
            _logger.LogError(errorMessage.ToString());

            return Task.CompletedTask;
        }
    }
}
