﻿using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using order.domain.AggregateModels.BuyerAggregate;
using order.domain.AggregateModels.OrderAggregate;
using order.domain.Contracts;
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
                    var order = new Order(
                        checkOutEvent.OrderInformationModel.CustomerId,
                        checkOutEvent.OrderInformationModel.BasketId,
                        checkOutEvent.MessageId,
                        checkOutEvent.OrderInformationModel.Total,
                        checkOutEvent.CorrelationToken
                    );

                    _orderId = order.OrderId;

                    // Project line items into OrderDetail method exposed in the root Order aggregate.
                    // This practice respects that Order is the root in the Order aggregate.
                    //foreach (var item in createOrderCommand.OrderDetails)
                    //    order.AddOrderItem(item.Title, item.AlbumId, item.Artist, item.Quantity, item.UnitPrice);

                    foreach (var item in checkOutEvent.OrderInformationModel.LineItems)
                        order.AddOrderItem(
                            item.Title,
                            item.Artist,
                            item.ProductId, 
                            item.Quantity, 
                            item.UnitPrice
                    );

                    // Create Buyer domain aggregate
                    var buyer = new Buyer(
                        checkOutEvent.OrderInformationModel.Buyer.Username,
                        checkOutEvent.OrderInformationModel.Buyer.FirstName,
                        checkOutEvent.OrderInformationModel.Buyer.LastName,    
                        checkOutEvent.OrderInformationModel.Buyer.Address, 
                        checkOutEvent.OrderInformationModel.Buyer.City,    
                        checkOutEvent.OrderInformationModel.Buyer.State,   
                        checkOutEvent.OrderInformationModel.Buyer.PostalCode,  
                        checkOutEvent.OrderInformationModel.Buyer.Phone,   
                        checkOutEvent.OrderInformationModel.Buyer.Email
                    );

                    // Add payment method for Buyer
                    var payment = new PaymentMethod(
                        checkOutEvent.OrderInformationModel.Payment.CreditCardNumber,
                        checkOutEvent.OrderInformationModel.Payment.SecurityCode,
                        checkOutEvent.OrderInformationModel.Payment.CardholderName,
                        checkOutEvent.OrderInformationModel.Payment.ExpirationDate
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