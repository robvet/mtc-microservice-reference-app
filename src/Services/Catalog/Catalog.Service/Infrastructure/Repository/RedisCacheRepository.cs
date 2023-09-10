using catalog.service.Contracts;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace catalog.service.Infrastructure.Repository
{
    public class RedisCacheRepository : IDistributedCacheRepository
    {
        private readonly ILogger<RedisCacheRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        
        public RedisCacheRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
        {
            _logger = loggerFactory.CreateLogger<RedisCacheRepository>();
            _redis = redis;
            _database = redis.GetDatabase();
        }

        // Delete collection, when data changes are made to products, artists, genres, or mediums
        public async Task<bool> DeleteCollectionAsync<T>(string key,
                                                         string correlationToken, 
                                                         TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await _database.KeyDeleteAsync(key);
                success = true;
                return result;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error deleting {typeof(T).Name} with id {key} for CorrelationToken {correlationToken}: {ex.Message}";
                telemetryClient.TrackException(ex);
                _logger.LogError(errorMessage);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "Delete", typeof(T).Name, startTime, timer.Elapsed, success);
            }
        }

        // Returns collection of T
        public async Task<List<T>> GetCollectionAsync<T>(string key,
                                                         string correlationToken,
                                                         TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var data = await _database.StringGetAsync(key);

                if (data.IsNullOrEmpty)
                {
                    success = false;
                    return default;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<T>>(data);
                    success = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error fetching {typeof(T).Name} with id {key} for CorrelationToken {correlationToken}: {ex.Message}";
                telemetryClient.TrackException(ex);
                _logger.LogError(errorMessage);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "GetAsync", typeof(T).Name, startTime, timer.Elapsed, success);
            }
        }

        public async Task<bool> SetCollectionAsync<T>(string key, 
                                                      List<T> items, 
                                                      string correlationToken, 
                                                      TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                //serialize the collection of T
                string serializedList = JsonConvert.SerializeObject(items, Formatting.Indented);

                var created =
                    await _database.StringSetAsync(key, serializedList);

                if (!created)
                {
                    _logger.LogInformation("Redis cache could not persist an item.");
                    return success;
                }
                else
                {
                    success = true;
                    _logger.LogInformation("Redis cache persisted item succesfully.");
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error updating {typeof(T).Name} with id {key} for CorrelationToken {correlationToken}: {ex.Message}";
                telemetryClient.TrackException(ex);
                _logger.LogError(errorMessage);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "UpdateAsync", typeof(T).Name, startTime, timer.Elapsed, success);
            }

            return success;
        }

        private IServer GetServer()
        {
            IServer server;

            try
            {
                var endpoint = _redis.GetEndPoints();
                server = _redis.GetServer(endpoint.First()); ;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not fetch Redis Server in RedisCacheRepository: {ex.Message}");
            }

            return server;
        }
    }
}
