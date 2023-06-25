using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using order.domain.AggregateModels.OrderAggregate;
using order.domain.Contracts;
using order.infrastructure.nosql;
using order.infrastructure.nosql.Persistence.Contracts;

namespace order.service.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrder2Repository _order2Repository;
        
        
        private readonly IOrderRepository _orderRepository;
        private readonly TelemetryClient _telemetryClient;

        public OrderQueries(IOrder2Repository order2Repository,
                            IOrderRepository orderRepository,
                            TelemetryClient telemetryInitializer)
        {
            _order2Repository = order2Repository;
            _orderRepository = orderRepository;
            _telemetryClient = telemetryInitializer;
        }

        // Query object allows a simple query against the datastore without having to 
        // adhere to the constraints of the domain Orderaggregate object. The query 
        // bypasses the Order domain object. Note how it uses a dynamic type.
        public async Task<OrderDto> GetOrder(string orderId,
                                            string corrleationId)
        {
            return await _order2Repository.GetById(orderId, corrleationId);
            //return await _orderRepository.GetById(orderId, corrleationId, _telemetryClient);
        }

        // adhere to the constraints of the domain Orderaggregate object. The query 
        // bypasses the Order domain object. Note how it uses a dynamic type.
        public async Task<IEnumerable<OrderDto>> GetOrders(string corrleationId)
        {
            //return await _order2Repository.GetItemsAsync();
            //return await _order2Repository.GetAll(corrleationId, _telemetryClient);
            return await _order2Repository.GetAll(corrleationId);
        }
    }
}