using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregateModels.BuyerAggregate;
using Ordering.Domain.AggregateModels.OrderAggregate;
using Ordering.Domain.Contracts;

namespace Ordering.API.Commands
{
    public class CreateOrderCommandHandler
    {
        //private readonly IConfiguration _configuration;
        private readonly ILogger<CreateOrderCommand> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private string _orderId;
        private string _correlationToken;
        private readonly TelemetryClient _telemetryClient;

        public CreateOrderCommandHandler(ILogger<CreateOrderCommand> logger,
                                         IServiceScopeFactory scopeFactory,
                                         TelemetryClient telemetryClient)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _telemetryClient = telemetryClient;
        }

        public async Task<string> Handle(CreateOrderCommand createOrderCommand)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    // Create scope to deal with 'scoped' lifetime of the DbContext object.
                    // Here, we're on a separate thread from the main thread and DbContext
                    // reference from main has been disposed. 
                    //https://github.com/mjrousos/MultiThreadedEFCoreSam

                    // Get references to read repository
                    var readModelOrderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                    // Create Order domain aggregate
                    var order = new Order(
                        createOrderCommand.UserName,
                        createOrderCommand.CheckoutId,
                        createOrderCommand.Total,
                        createOrderCommand.CorrelationToken
                    );

                    // Set correlationToken
                    _correlationToken = createOrderCommand.CorrelationToken;
                    
                    //_orderId = order.orderid;
                    
                    // Project line items into OrderDetail method exposed in the root Order aggregate.
                    // This practice respects that Order is the root in the Order aggregate.
                    foreach (var item in createOrderCommand.OrderDetails)
                        order.AddOrderItem(item.Title, item.AlbumId, item.Artist, item.Quantity, item.UnitPrice);

                    // Create Buyer domain aggregate
                    var buyer = new Buyer(
                        createOrderCommand.UserName,
                        createOrderCommand.FirstName,
                        createOrderCommand.LastName,
                        createOrderCommand.Address,
                        createOrderCommand.City,
                        createOrderCommand.State,
                        createOrderCommand.PostalCode,
                        createOrderCommand.Phone,
                        createOrderCommand.Email);

                    // Add payment method for Buyer
                    buyer.AddPaymentMethod(createOrderCommand.CreditCardNumber,
                        createOrderCommand.SecurityCode,
                        createOrderCommand.CardholderName,
                        createOrderCommand.ExpirationDate);

                    // Add Buyer information to the Order Aggregate so that we can do
                    // use EF's 'fixup" to perform a single insert into the dataStore
                    // for the entire graph.
                    order.Buyer = buyer;

                    // Add Order to ReadDataStore
                    await readModelOrderRepository.Add(order, _telemetryClient);

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
    }
}