using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Ordering.Domain.Contracts;

namespace Ordering.API.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;
        private readonly TelemetryClient _telemetryClient;

        public OrderQueries(IOrderRepository orderRepository,
                            TelemetryClient telemetryInitializer)
        {
            _orderRepository = orderRepository;
            _telemetryClient = telemetryInitializer;
        }

        // Query object allows a simple query against the datastore without having to 
        // adhere to the constraints of the domain Orderaggregate object. The query 
        // bypasses the Order domain object. Note how it uses a dynamic type.
        public async Task<dynamic> GetOrder(string orderId, 
                                            string corrleationId)
        {
            return await _orderRepository.GetById(orderId, corrleationId, _telemetryClient);
        }

        // adhere to the constraints of the domain Orderaggregate object. The query 
        // bypasses the Order domain object. Note how it uses a dynamic type.
        public async Task<dynamic> GetOrders(string corrleationId)
        {
            return await _orderRepository.GetAll(corrleationId, _telemetryClient);
        }
    }
}