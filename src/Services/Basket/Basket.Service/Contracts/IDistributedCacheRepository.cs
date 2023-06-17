using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace Basket.Service.Contracts
{
   public interface IDistributedCacheRepository 
    {
        Task<T> GetAsync<T>(Guid id,
                            TelemetryClient telemetryClient,
                            string methodName,
                            string correlationToken);
        
        Task<List<T>> GetAll<T>(string correlationToken, 
                                TelemetryClient telemetryClient);

        Task<bool> UpdateAsync<T>(T entity, 
                                  Guid id, 
                                  string correlationToken, 
                                  TelemetryClient telemetryClient);


        Task<bool> DeleteAsync<T>(Guid id,
                                  TelemetryClient telemetryClient,
                                  string correlationToken);

    }
}
