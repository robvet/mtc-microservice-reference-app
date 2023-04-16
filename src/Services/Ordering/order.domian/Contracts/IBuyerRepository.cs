using System.Threading.Tasks;
using order.domian.AggregateModels.BuyerAggregate;

namespace order.domian.Contracts
{
    public interface IBuyerRepository
    {
        Task Add(Buyer entity);
    }
}