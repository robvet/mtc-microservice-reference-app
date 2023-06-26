using Microsoft.ApplicationInsights;
using order.domain.AggregateModels.OrderAggregate;
using order.infrastructure.nosql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace order.service.Queries
{
    public interface IOrderQueries
    {
        Task<OrderDto> GetByOrderId(string orderId, string corrleationId);

        Task<IEnumerable<OrderDto>> GetAll(string corrleationId);
    }
}