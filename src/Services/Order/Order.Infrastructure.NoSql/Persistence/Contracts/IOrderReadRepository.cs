namespace order.infrastructure.nosql.Persistence.Contracts
{
    using order.domain.AggregateModels.OrderAggregate;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderReadRepository : IRepository<OrderDto>
    {
        Task<IEnumerable<OrderDto>> GetAll(string correlationId);
        Task<IEnumerable<OrderDto>> GetByOrderId(string Id, string correlationId);
        Task<OrderDto> GetByResourceId(string Id, string correlationId);
    }
}