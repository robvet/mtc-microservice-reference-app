using Basket.Service.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace Basket.Service.Infrastructure.Repository
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

        public async Task<bool> DeleteAsync<T>(Guid id, 
                                               TelemetryClient telemetryClient,
                                               string correlationToken)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var result = await _database.KeyDeleteAsync(id.ToString());
                success = true;
                return result;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error deleting {typeof(T).Name} with id {id} for CorrelationToken {correlationToken}: {ex.Message}";
                telemetryClient.TrackException(ex);
                _logger.LogError(errorMessage);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "Delete", typeof(T).Name, startTime, timer.Elapsed, success);
            }
        }

        public async Task<T> GetAsync<T>(Guid id,
                                 TelemetryClient telemetryClient,
                                 string methodName,
                                 string correlationToken)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var data = await _database.StringGetAsync(id.ToString());

                if (data.IsNullOrEmpty)
                {
                    success = false;
                    return default;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<T>(data);
                    success = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error fetching {typeof(T).Name} with id {id} for CorrelationToken {correlationToken}: {ex.Message}";
                telemetryClient.TrackException(ex);
                _logger.LogError(errorMessage);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "GetAsync", typeof(T).Name, startTime, timer.Elapsed, success);
            }
        }
         
        public async Task<List<T>> GetAll<T>(string correlationToken, TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var allItems = new List<T>();
            string guidValue = null;

            try
            {
                var server = GetServer();
                var data = server.Keys();

                foreach (string key in data)
                {
                    guidValue = key;

                    // Ensure value can parse to Guid and that Guid value is not empty
                    if (Guid.TryParse(guidValue, out Guid guid) && guid != Guid.Empty)
                    {
                        var item = await GetAsync<T>(guid, telemetryClient, "GetAll", correlationToken);
                        allItems.Add(item);
                    }
                }

                success = true;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error fetching all {typeof(T).Name} with id {guidValue} for CorrelationToken {correlationToken}: {ex.Message}";
                telemetryClient.TrackException(ex);
                _logger.LogError(errorMessage);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "GetAll", typeof(T).Name, startTime, timer.Elapsed, success);
            }

            return allItems;
        }

        public async Task<bool> UpdateAsync<T>(T entity, Guid id, string correlationToken, TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var created =
                    await _database.StringSetAsync(id.ToString(), JsonConvert.SerializeObject(entity));

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
                var errorMessage = $"Error updating {typeof(T).Name} with id {id} for CorrelationToken {correlationToken}: {ex.Message}";
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


        //private async Task<T> GetAsync<T>(Guid id)
        //{
        //    var data = await _database.StringGetAsync(id.ToString());

        //    if (data.IsNullOrEmpty)
        //    {
        //        return default;
        //    }

        //    return JsonConvert.DeserializeObject<T>(data);
        //}

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
