using System.Threading.Tasks;
using Ordering.Domain.AggregateModels.BuyerAggregate;

namespace Ordering.Domain.Contracts
{
    public interface IBuyerRepository
    {
        Task Add(Buyer entity);
    }
}