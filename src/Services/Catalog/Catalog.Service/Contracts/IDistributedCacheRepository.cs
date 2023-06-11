using catalog.service.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace catalog.service.Contracts
{
    public interface IDistributedCacheRepository
    {
        Task<BasketEntity> GetBasketAsync(string basketid,
                                                          string correlationToken,
                                                          TelemetryClient telemetryClient);

        Task<List<BasketEntity>> GetAllBaskets(string correlationToken,
                                                               TelemetryClient telemetryClient);

        //IEnumerable<string> GetUsers();

        Task<BasketEntity> UpdateBasketAsync(BasketEntity basketEntity,
                                                             string correlationToken,
                                                             TelemetryClient telemetryClient);

        Task<bool> DeleteBasketAsync(string id,
                                     TelemetryClient telemetryClient);

    }
}
