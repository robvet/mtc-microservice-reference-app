using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregateModels.OrderAggregate;
using Ordering.Infrastructure.Relational.Contracts;
using Ordering.Infrastructure.Relational.DataStore;

namespace Ordering.Infrastructure.Relational.Repository
{
    public class OrderRelationalRepository : BaseRepository<Order>, IOrderRelationalRepository
    {
        public OrderRelationalRepository(DataContext ctx) : base(ctx)
        {
        }

        public async Task<int> GetCount(string correlationToken)
        {
            return await Get().CountAsync();
        }
    }
}