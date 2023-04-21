using System.Threading.Tasks;
using order.domain.AggregateModels.BuyerAggregate;

namespace order.domain.Contracts
{
    public interface IBuyerRepository
    {
        Task Add(Buyer entity);
    }
}