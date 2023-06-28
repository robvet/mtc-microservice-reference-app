namespace order.infrastructure.nosql.Persistence.Contracts
{
    using order.domain.Models.ReadModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderReadRepository : IRepository<OrderReadModel>
    {
        Task<IEnumerable<OrderReadModel>> GetAll(string correlationId);
        Task<OrderReadModel> GetByOrderId(string Id, string correlationId);
        Task<OrderReadModel> GetByResourceId(string Id, string correlationId);
    }
}