using EventBus.Bus;
using EventBus.Events;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using order.service.Commands;
using order.service.Dtos;

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
                    $"Event: CheckOutEventHandler invoked: BasketID:{checkOutEvent.OrderInformationModel.BasketId}");

                var createOrderCommand = new CreateOrderCommand(
                    checkOutEvent.OrderInformationModel.BasketId,
                    checkOutEvent.OrderInformationModel.CustomerId,
                    checkOutEvent.MessageId,
                    checkOutEvent.OrderInformationModel.Buyer.Username,
                    checkOutEvent.OrderInformationModel.Total,
                    checkOutEvent.OrderInformationModel.Buyer.FirstName,
                    checkOutEvent.OrderInformationModel.Buyer.LastName,
                    checkOutEvent.OrderInformationModel.Buyer.Address,
                    checkOutEvent.OrderInformationModel.Buyer.City,
                    checkOutEvent.OrderInformationModel.Buyer.State,
                    checkOutEvent.OrderInformationModel.Buyer.PostalCode,
                    checkOutEvent.OrderInformationModel.Buyer.Phone,
                    checkOutEvent.OrderInformationModel.Buyer.Email,
                    checkOutEvent.OrderInformationModel.Payment.CreditCardNumber,
                    checkOutEvent.OrderInformationModel.Payment.SecurityCode,
                    checkOutEvent.OrderInformationModel.Payment.CardholderName,
                    checkOutEvent.OrderInformationModel.Payment.ExpirationDate,
                    checkOutEvent.CorrelationToken = correlationToken,
                    // Is this cool Linq? Generated from Resharper. It iterates through lineItem collection and projects an orderDetailDto for each item
                    // Map collection of CheckOutEventLineItems to collection of OderDetailDtos
                    checkOutEvent.OrderInformationModel.LineItems.Select(lineItem => new OrderDetailDto
                    {
                        Artist = lineItem.Artist,
                        Title = lineItem.Title,
                        Quantity = lineItem.Quantity,
                        UnitPrice = decimal.Parse(lineItem.UnitPrice),
                        AlbumId = lineItem.ProductId
                    }).ToList()
                );

                // Invoke Command that creates order
                var orderId = await _orderCommandHandler.Handle(createOrderCommand);

                //checkedOutEvent.OrderInformationModel.OrderSystemId = orderId;

                _telemetryClient.TrackEvent(
                    $"Event: CheckOutEventHandler: Buyer created:{orderId}");

                //************** Publish Event  *************************
                // Publish event to clear basket for this order from Basket service
                var emptyCartEvent = new EmptyBasketEvent
                {
                    BasketID = checkOutEvent.OrderInformationModel.BasketId,
                    CorrelationToken = correlationToken
                };

                _logger.LogInformation(
                    $"Publishing EmptyBasketEvent from CheckOutEventHandler in Ordering.API for Request {correlationToken} ");

                _telemetryClient.TrackEvent(
                    $"Event: Publishing EmptyBasketEvent from CheckOutEventHandler for orderid:{orderId}");

                await _eventBusPublisher.Publish<EmptyBasketEvent>(emptyCartEvent);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Exception in CheckOutEventHandler: {ex.Message} for Request {correlationToken}");
            }

            await Task.CompletedTask;
        }
    }
}