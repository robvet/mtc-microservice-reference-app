using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using order.domain.Models.ReadModels;
using order.infrastructure.nosql;
using order.infrastructure.nosql.Persistence.Contracts;

namespace order.service.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly TelemetryClient _telemetryClient;

        public OrderQueries(IOrderReadRepository orderReadRepository,
                            TelemetryClient telemetryInitializer)
        {
            _orderReadRepository = orderReadRepository;
            _telemetryClient = telemetryInitializer;
        }

        // Query object allows a simple query against the datastore without having to 
        // adhere to the constraints of the domain Orderaggregate object. The query 
        // bypasses the Order domain object. Note how it uses a OrderDto type.
        //public async Task<IEnumerable<OrderDto>> GetByOrderId(string orderId,
        //                                    string corrleationId)
        //{
        //    return await _order2Repository.GetByResourceId(orderId, corrleationId);
        //    //return await _order2Repository.GetByOrderId(orderId, corrleationId);
        //    //return await _orderRepository.GetById(orderId, corrleationId, _telemetryClient);
        //}

        public async Task<OrderReadModel> GetByOrderId(string orderId, string corrleationId)
        {
            return await _orderReadRepository.GetByOrderId(orderId, corrleationId);
           
            
            
            //return await _order2Repository.GetByOrderId(orderId, corrleationId);
            //return await _orderRepository.GetById(orderId, corrleationId, _telemetryClient);
        }



        // adhere to the constraints of the domain Orderaggregate object. The query 
        // bypasses the Order domain object. Note how it uses a OrderDto type.
        public async Task<IEnumerable<OrderReadModel>> GetAll(string corrleationId)
        {
            //return await _order2Repository.GetItemsAsync();
            //return await _order2Repository.GetAll(corrleationId, _telemetryClient);
            return await _orderReadRepository.GetAll(corrleationId);
        }
    }
}