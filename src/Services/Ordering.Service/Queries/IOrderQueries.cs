using Microsoft.ApplicationInsights;
using System.Threading.Tasks;

namespace Ordering.API.Queries
{
    public interface IOrderQueries
    {
        Task<dynamic> GetOrder(string orderId, string corrleationId);

        Task<dynamic> GetOrders(string corrleationId);
    }
}