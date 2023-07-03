using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using order.domain.Models.BuyerAggregateModels;
using order.domain.Models.OrderAggregateModels;
using order.infrastructure.nosql.Persistence.Contracts;
using order.service.Events;

namespace order.service.Commands
{
    public class CreateOrderCommandHandler
    {
        //private readonly IConfiguration _configuration;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Guid _orderId;
        private string _correlationToken;
        private readonly TelemetryClient _telemetryClient;
        private readonly IOrderWriteRepository _orderWriteRepository;

        public CreateOrderCommandHandler(IOrderWriteRepository orderWriteRepository,
                                         ILogger<CreateOrderCommandHandler> logger,
                                         IServiceScopeFactory scopeFactory,
                                         TelemetryClient telemetryClient)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _telemetryClient = telemetryClient;
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task<Guid> Handle(CheckOutEvent checkOutEvent)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    // Set correlationToken
                    _correlationToken = checkOutEvent.CorrelationToken;

                    // Create scope to deal with 'scoped' lifetime of the DbContext object.
                    // Here, we're on a separate thread from the main thread and DbContext
                    // reference from main has been disposed. 
                    //https://github.com/mjrousos/MultiThreadedEFCoreSam

                    // Create Order domain aggregate
                    var order = new Order(checkOutEvent.checkOutEventModel.CustomerId,
                                          checkOutEvent.checkOutEventModel.BasketId,
                                          checkOutEvent.MessageId,
                                          checkOutEvent.checkOutEventModel.Total,
                                          checkOutEvent.CorrelationToken
                    );

                    _orderId = order.OrderId;

                    // Project line items into OrderDetail method exposed in the root Order aggregate.
                    // This practice respects that Order is the root in the Order aggregate.
                    //foreach (var item in createOrderCommand.OrderDetails)
                    //    order.AddOrderItem(item.Title, item.AlbumId, item.Artist, item.Quantity, item.UnitPrice);

                    foreach (var item in checkOutEvent.checkOutEventModel.LineItems)
                        order.AddOrderItem(item.ProductId,
                                           item.Title,
                                           item.ArtistId,
                                           item.Artist,
                                           item.GenreId,
                                           item.Genre,
                                           item.UnitPrice,
                                           item.Quantity,
                                           item.Condition,
                                           item.Status,
                                           item.MediumId,
                                           item.Medium,
                                           item.DateCreated,
                                           item.HighValueItem
                    );

                    // Create Buyer domain aggregate
                    var buyer = new Buyer(checkOutEvent.checkOutEventModel.Buyer.Username,
                                          checkOutEvent.checkOutEventModel.Buyer.FirstName,
                                          checkOutEvent.checkOutEventModel.Buyer.LastName,    
                                          checkOutEvent.checkOutEventModel.Buyer.Address, 
                                          checkOutEvent.checkOutEventModel.Buyer.City,    
                                          checkOutEvent.checkOutEventModel.Buyer.State,   
                                          checkOutEvent.checkOutEventModel.Buyer.PostalCode,  
                                          checkOutEvent.checkOutEventModel.Buyer.Phone,   
                                          checkOutEvent.checkOutEventModel.Buyer.Email
                    );

                    // Add payment method for Buyer
                    var payment = new PaymentMethod(checkOutEvent.checkOutEventModel.Payment.CreditCardNumber,
                                          checkOutEvent.checkOutEventModel.Payment.SecurityCode,
                                          checkOutEvent.checkOutEventModel.Payment.CardholderName,
                                          checkOutEvent.checkOutEventModel.Payment.ExpirationDate
                    );
                    
                    var orderStatus = new OrderStatus(
                        (int)OrderStatusEnum.Pending,
                        OrderStatusEnum.Pending.ToString()
                    ); 


                    // Add supporting entities to Order Aggregate so that we can do
                    // use EF's 'fixup" to perform a single insert into the dataStore
                    // for the entire graph.
                    order.Buyer = buyer;
                    order.PaymentMethod = payment;  
                    order.OrderStatus = orderStatus;

                    // Add Order to ReadDataStore
                    var createdOrder = await _orderWriteRepository.CreateOrder(order, order.CorrelationToken);

                    //await readModelOrderRepository.Add(order, _telemetryClient);

                    _logger.LogInformation($"Created new order for Request {order.CorrelationToken} ");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating Order: {ex.Message} for Request {_correlationToken} ");
                    throw;
                }

                return _orderId;
            }
        }









        //    public async Task<string> Handle(CreateOrderCommand createOrderCommand)
        //{
        //    using (var scope = _scopeFactory.CreateScope())
        //    {
        //        try
        //        {
        //            // Set correlationToken
        //            _correlationToken = createOrderCommand.CorrelationToken;

        //            // Create scope to deal with 'scoped' lifetime of the DbContext object.
        //            // Here, we're on a separate thread from the main thread and DbContext
        //            // reference from main has been disposed. 
        //            //https://github.com/mjrousos/MultiThreadedEFCoreSam

        //            // Get references to read repository
        //            var readModelOrderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        //            // Create Order domain aggregate
        //            var order = new Order(
        //                createOrderCommand.CustomerId,
        //                createOrderCommand.BasketId,
        //                createOrderCommand.MessageId, 
        //                createOrderCommand.Total,
        //                createOrderCommand.CorrelationToken
        //            );
                                        
        //            //_orderId = order.orderid;

        //            // Project line items into OrderDetail method exposed in the root Order aggregate.
        //            // This practice respects that Order is the root in the Order aggregate.
        //            foreach (var item in createOrderCommand.OrderDetails)
        //                order.AddOrderItem(
        //                    item.Title, 
        //                    item.AlbumId, 
        //                    item.Artist, 
        //                    item.Quantity, 
        //                    item.UnitPrice
        //                );

        //            // Create Buyer domain aggregate
        //            var buyer = new Buyer(
        //                createOrderCommand.UserName,
        //                createOrderCommand.FirstName,
        //                createOrderCommand.LastName,
        //                createOrderCommand.Address,
        //                createOrderCommand.City,
        //                createOrderCommand.State,
        //                createOrderCommand.PostalCode,
        //                createOrderCommand.Phone,
        //                createOrderCommand.Email);

        //            // Add payment method for Buyer
        //            buyer.AddPaymentMethod(createOrderCommand.CreditCardNumber,
        //                createOrderCommand.SecurityCode,
        //                createOrderCommand.CardholderName,
        //                createOrderCommand.ExpirationDate);

        //            // Add Buyer information to the Order Aggregate so that we can do
        //            // use EF's 'fixup" to perform a single insert into the dataStore
        //            // for the entire graph.
        //            order.Buyer = buyer;

        //            // Add Order to ReadDataStore
        //            await readModelOrderRepository.Add(order, _telemetryClient);

        //            _logger.LogInformation($"Created new order for Request {order.CorrelationToken} ");
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError($"Error creating Order: {ex.Message} for Request {_correlationToken} ");
        //            throw;
        //        }

        //        return _orderId;
        //    }
        //}
    }
}