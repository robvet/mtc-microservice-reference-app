using EventBus.Bus;
using EventBus.Events;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using order.service.Commands;
using EventBus.EventModels;

namespace order.service.Events
{
    public class CheckOutEventHandler : IMessageEventHandler
    {
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly ILogger<CheckOutEventHandler> _logger;
        private readonly CreateOrderCommandHandler _orderCommandHandler;
        private readonly TelemetryClient _telemetryClient;

        public CheckOutEventHandler(IEventBusPublisher eventBusPublisher,
            CreateOrderCommandHandler orderCommandHandler,
            ILogger<CheckOutEventHandler> logger,
            TelemetryClient telemetryClient)
        {
            _eventBusPublisher = eventBusPublisher;
            _orderCommandHandler = orderCommandHandler;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public async Task HandleAsync(MessageEvent messageEvent)
        {
            // https://stackoverflow.com/questions/43124722/application-insights-custom-trackrequest-is-creating-duplicate-messages
            var requestTelemetry = new RequestTelemetry
            {
                Name = "CheckOutEventHandler invoked from EventBus"
            };

            _telemetryClient.TrackRequest(requestTelemetry);

            string correlationToken = null;

            try
            {
                correlationToken = messageEvent.CorrelationToken;
                
                var checkOutEvent = messageEvent as CheckOutEvent;

                _logger.LogInformation($"Invoked CheckOutEventHandler in Ordering.API for Request {correlationToken} ");

                _telemetryClient.TrackEvent(
                    $"Event: Invoked CheckOutEventHandler in Ordering.API for Request {correlationToken}");

                if (checkOutEvent == null)
                {
                    _logger.LogError(
                        $"Publishing EmptyBasketEvent from CheckOutEventHandler in Ordering.API for Request {correlationToken} ");
                    throw new Exception(
                        $"Exception in CheckOutEventHandler: 'CheckOutEvent is Null' for Request {correlationToken}");
                }

                _telemetryClient.TrackEvent(
                    $"Event: CheckOutEventHandler invoked: BasketID:{checkOutEvent.checkOutEventModel.BasketId}");

                // ************** Create Order  *************************
                // Down stream call to create order in Order service
                var orderId = await _orderCommandHandler.Handle(checkOutEvent);

                _telemetryClient.TrackEvent(
                    $"Event: CheckOutEventHandler: Buyer created:{orderId}");

                //************** Publish Event  *************************
                // Create event to update shopping basket status
                var basketProcessedEvent = new BasketProcessedEvent
                {
                    basketProcessedEventModel = new BasketProcessedEventModel
                    {
                        BasketID = checkOutEvent.checkOutEventModel.BasketId
                    },
                    CorrelationToken = correlationToken
                };

                _logger.LogInformation(
                    $"Publishing BasketProcessedEvent from CheckOutEventHandler in Ordering.API for Request {correlationToken} ");

                _telemetryClient.TrackEvent(
                    $"Event: Publishing BasketProcessedEvent from CheckOutEventHandler for orderid:{orderId}");

                // Public event to clear basket for this order from Basket service
                await _eventBusPublisher.Publish<BasketProcessedEventModel>(basketProcessedEvent);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Exception in Order Creation process thrown in CheckOutEventHandler: {ex.Message} for Request {correlationToken}");
            }

            await Task.CompletedTask;
        }
    }
}