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
 
    public class ServiceBusEventPublisher : IEventBusPublisher
    {
        private readonly string _boundServiceName;
        private readonly ILogger<ServiceBusEventPublisher> _logger;
        private TopicClient _topicClient;
        private string _correlationToken;
        private string _currentMessageType;
        
        public ServiceBusEventPublisher(string connectionString,
            string boundServiceName,
            ILogger<ServiceBusEventPublisher> logger
            )
        {
            _logger = logger;
            _boundServiceName = boundServiceName;
            CreateTopicClient(connectionString, logger);
        }

        private void CreateTopicClient(string connectionString, ILogger logger)
        {
            try
            {
                //_topicClient = new TopicClient(connectionString, topicName, RetryPolicy.Default);

                var connectionBuilder = new ServiceBusConnectionStringBuilder(connectionString ?? throw new ArgumentNullException($"ServiceBus ConnectionSecret for Basket is Null"));
                
                _topicClient = new TopicClient(connectionBuilder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot create TopicClient in ServiceBusEventProvider: {ex.Message}");
            }
        }

        /// <summary>
        /// Publish message to Azure ServiceBus
        /// </summary>
        public async Task Publish<T>(MessageEvent messageEvent)
        {
            try
            {
                var jsonMessage = JsonConvert.SerializeObject(messageEvent);

                var message = new Message
                {
                    // Set application-defined value that uniquely identifies the message and its payload
                    // If enabled, the duplicate detection feature identifies and removes second and
                    //further submissions of messages with the same MessageId.
                    MessageId = Guid.NewGuid().ToString(), 
                    
                    Body = Encoding.UTF8.GetBytes(jsonMessage),
                    UserProperties =
                    {
                        {"Event", typeof(T).Name},
                        {"_correlationToken", messageEvent.CorrelationToken}
                    }
                };

                _logger.LogInformation(
                    $"Publishing {typeof(T).Name} with CorrelationToken {messageEvent.CorrelationToken} for Service {_boundServiceName}");

                await _topicClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(ex.HResult),
                    ex,
                    $"Exception throw in Publish for Service {_boundServiceName} for Event {typeof(T).Name} for CorrelationToken {messageEvent.CorrelationToken} : {ex.Message}");
                throw;
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

            errorMessage.Append($"Exception throw in ServiceBusEventProvider-MessageHandler for Event {_currentMessageType} for Service {_boundServiceName} and Correlation Token {_correlationToken}; Error message = {errorMessage}");
            _logger.LogError(errorMessage.ToString());

            return Task.CompletedTask;
        }
    }
}
