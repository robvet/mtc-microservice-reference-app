using order.domain.Models.ReadModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace order.service.Queries
{
    public interface IOrderQueries
    {
        Task<OrderReadModel> GetByOrderId(string orderId, string corrleationId);
        Task<IEnumerable<OrderReadModel>> GetAll(string corrleationId);
    }
}