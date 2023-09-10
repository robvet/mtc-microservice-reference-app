using catalog.service.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using System;

namespace catalog.service.Contracts
{
    public interface IDistributedCacheRepository
    {
        Task<List<T>> GetCollectionAsync<T>(string key, string correlationToken, TelemetryClient telemetryClient);

        Task<bool> SetCollectionAsync<T>(string key, List<T> genres, string correlationToken, TelemetryClient telemetryClient);

        Task<bool> DeleteCollectionAsync<T>(string key, string correlationToken, TelemetryClient telemetryClient);
    }
}
