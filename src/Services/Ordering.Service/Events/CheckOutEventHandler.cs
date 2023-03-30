using EventBus.Bus;
using EventBus.Events;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using Ordering.API.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ordering.API.Dtos;

namespace Ordering.API.Events
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

                var checkedOutEvent = messageEvent as CheckOutEvent;

                _logger.LogInformation($"Invoked CheckOutEventHandler in Ordering.API for Request {correlationToken} ");

                _telemetryClient.TrackEvent(
                    $"Event: Invoked CheckOutEventHandler in Ordering.API for Request {correlationToken}");

                if (checkedOutEvent == null)
                {
                    _logger.LogError(
                        $"Publishing EmptyBasketEvent from CheckOutEventHandler in Ordering.API for Request {correlationToken} ");
                    throw new Exception(
                        $"Exception in CheckOutEventHandler: 'CheckOutEvent is Null' for Request {correlationToken}");
                }
                
                _telemetryClient.TrackEvent(
                    $"Event: CheckOutEventHandler invoked: BasketID:{checkedOutEvent.OrderInformationModel.BasketId}");

                var createOrderCommand = new CreateOrderCommand(
                    checkedOutEvent.OrderInformationModel.BasketId,
                    checkedOutEvent.OrderInformationModel.CheckoutId,
                    checkedOutEvent.OrderInformationModel.Buyer.Username,
                    checkedOutEvent.OrderInformationModel.Total,
                    checkedOutEvent.OrderInformationModel.Buyer.FirstName,
                    checkedOutEvent.OrderInformationModel.Buyer.LastName,
                    checkedOutEvent.OrderInformationModel.Buyer.Address,
                    checkedOutEvent.OrderInformationModel.Buyer.City,
                    checkedOutEvent.OrderInformationModel.Buyer.State,
                    checkedOutEvent.OrderInformationModel.Buyer.PostalCode,
                    checkedOutEvent.OrderInformationModel.Buyer.Phone,
                    checkedOutEvent.OrderInformationModel.Buyer.Email,
                    checkedOutEvent.OrderInformationModel.Payment.CreditCardNumber,
                    checkedOutEvent.OrderInformationModel.Payment.SecurityCode,
                    checkedOutEvent.OrderInformationModel.Payment.CardholderName,
                    checkedOutEvent.OrderInformationModel.Payment.ExpirationDate,
                    checkedOutEvent.CorrelationToken = correlationToken,
                    // Is this cool Linq? Generated from Resharper. It iterates through lineItem collection and projects an orderDetailDto for each item
                    // Map collection of CheckOutEventLineItems to collection of OderDetailDtos
                    checkedOutEvent.OrderInformationModel.LineItems.Select(lineItem => new OrderDetailDto
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

                checkedOutEvent.OrderInformationModel.OrderSystemId = orderId;
                
                _telemetryClient.TrackEvent(
                    $"Event: CheckOutEventHandler: Buyer created:{orderId}");
                
                //************** Publish Event  *************************
                // Publish event to clear basket for this order from Basket service
                var emptyCartEvent = new EmptyBasketEvent
                {
                    BasketID = checkedOutEvent.OrderInformationModel.BasketId,
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