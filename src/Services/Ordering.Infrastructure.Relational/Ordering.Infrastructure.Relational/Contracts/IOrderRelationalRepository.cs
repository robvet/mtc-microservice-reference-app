using System.Threading.Tasks;
using Ordering.Domain.AggregateModels.OrderAggregate;

namespace Ordering.Infrastructure.Relational.Contracts
{
    public interface IOrderRelationalRepository : IRelationalRepository<Order>
    {
        Task<int> GetCount(string correlationToken);
    }
}