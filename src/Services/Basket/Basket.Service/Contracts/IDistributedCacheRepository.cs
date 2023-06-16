using Basket.Service.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Service.Domain.Entities;
using Microsoft.ApplicationInsights;
using basket.service.Contracts;

namespace Basket.Service.Contracts
{
   public interface IDistributedCacheRepository 
    {
        //Task<BasketEntity> GetBasketAsync(string basketid, 
        //                                                  string correlationToken, 
        //                                                  TelemetryClient telemetryClient);

        Task<T> GetAsync<T>(Guid id,
                            TelemetryClient telemetryClient,
                            string methodName,
                            string correlationToken);
        
        //Task<List<BasketEntity>> GetAllBaskets(string correlationToken,
        //                                                       TelemetryClient telemetryClient);

        Task<List<T>> GetAll<T>(string correlationToken, TelemetryClient telemetryClient);

        //IEnumerable<string> GetUsers();

        //Task<BasketEntity> UpdateBasketAsync(BasketEntity basketEntity, 
        //                                                     string correlationToken,
        //                                                     TelemetryClient telemetryClient);

        Task<bool> UpdateAsync<T>(T entity, Guid id, string correlationToken, TelemetryClient telemetryClient);


        //Task<bool> DeleteBasketAsync(string id, TelemetryClient telemetryClient);

        Task<bool> DeleteAsync<T>(Guid id, TelemetryClient telemetryClient);

        //Task<T> UpdateBasketAsync<T>(T item, string correlationToken, TelemetryClient telemetryClient) where T : IReadModelEntity;

    }
}
