using Basket.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Domain.Entities;
using Microsoft.ApplicationInsights;

namespace Basket.API.Contracts
{
   public interface IDistributedCacheRepository 
    {
        Task<Domain.Entities.BasketEntity> GetBasketAsync(string basketid, 
                                                          string correlationToken, 
                                                          TelemetryClient telemetryClient);
        
        Task<List<Domain.Entities.BasketEntity>> GetAllBaskets(string correlationToken,
                                                               TelemetryClient telemetryClient);
        
        //IEnumerable<string> GetUsers();
       
        Task<Domain.Entities.BasketEntity> UpdateBasketAsync(Domain.Entities.BasketEntity basketEntity, 
                                                             string correlationToken,
                                                             TelemetryClient telemetryClient);
       
        Task<bool> DeleteBasketAsync(string id,
                                     TelemetryClient telemetryClient);

    }
}
