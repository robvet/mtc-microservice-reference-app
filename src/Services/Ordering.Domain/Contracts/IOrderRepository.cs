using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Ordering.Domain.AggregateModels.OrderAggregate;

namespace Ordering.Domain.Contracts
{
    public interface IOrderRepository
    {
        Task<string> Add(Order entity, TelemetryClient telemetryClient);
        Task<dynamic> GetById(string id, string correlationToken, TelemetryClient telemetryClient);
        Task<dynamic> GetAll(string correlationToken, TelemetryClient telemetryClient);
    }
}