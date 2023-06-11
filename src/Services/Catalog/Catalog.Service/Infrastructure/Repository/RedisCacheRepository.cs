using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using catalog.service.Contracts;
using catalog.service.Domain.Entities;

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

        public async Task<bool> DeleteBasketAsync(string id, TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                return await _database.KeyDeleteAsync(id);
                success = true;
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "DeleteBasket", success.ToString(), startTime, timer.Elapsed, success);
            }

            return success;
        }

        public async Task<BasketEntity> GetBasketAsync(string basketid,
                                                       string correlationToken,
                                                       TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue = "Not Found";

            try
            {
                var data = await _database.StringGetAsync(basketid);

                if (data.IsNullOrEmpty)
                {
                    return null;
                }
                else
                {
                    var basketdata = JsonConvert.DeserializeObject<BasketEntity>(data);
                    success = true;
                    returnValue = "Found";
                    return basketdata;
                }
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "GetBasket", returnValue, startTime, timer.Elapsed, success);
            }

            //var data = await _redisDatabase.StringGetAsync(basketid);
            //if (data.IsNullOrEmpty)
            //{
            //    return null;
            //}
            //var basketdata = JsonConvert.DeserializeObject<BasketEntity>(data);
            //return basketdata;
        }

        public async Task<BasketEntity> UpdateBasketAsync(BasketEntity basketEntity,
                                                          string correlationToken,
                                                          TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue = "Not Found";

            try
            {
                var created =
                    await _database.StringSetAsync(basketEntity.BasketId, JsonConvert.SerializeObject(basketEntity));











                //https://stackoverflow.com/questions/28702008/azure-redis-cache-batch-operations-multiple-operations

                ////////        var pairs = new KeyValuePair<RedisKey, RedisValue>[] {
                ////////    new KeyValuePair<RedisKey,RedisValue>("key1", "value1"),
                ////////    new KeyValuePair<RedisKey,RedisValue>("key2", "value2"),
                ////////    new KeyValuePair<RedisKey,RedisValue>("key3", "value3"),
                ////////    new KeyValuePair<RedisKey,RedisValue>("key4", "value4"),
                ////////    new KeyValuePair<RedisKey,RedisValue>("key5", "value5"),
                ////////};

                ////////        var keys = pairs.Select(p => p.Key).ToArray();

                ////////        Connection.GetDatabase().StringSet(pairs);

                ////////        var values = Connection.GetDatabase().StringGet(keys);

                ////////        await _redisDatabase.StringSetAsync(pairs);









                // _redisDatabase.SetAdd()

                if (!created)
                {
                    _logger.LogInformation("Redis cache could not persist an item.");
                    return null;
                }
                else
                {
                    success = true;
                    returnValue = basketEntity.BasketId;
                    _logger.LogInformation("Redis cache persisted item succesfully.");
                }
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                _logger.LogError(ex.ToString());
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "UpdateBasket", returnValue, startTime, timer.Elapsed, success);
            }

            return await GetBasketAsync(returnValue);
            //return null;
        }

        public async Task<List<BasketEntity>> GetAllBaskets(string correlationToken,
                                                             TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue = "Not Found";

            var basketall = new List<BasketEntity>();

            try
            {
                var server = GetServer();
                var data = server.Keys();

                foreach (string key in data)
                {

                    var basket = await GetBasketAsync(key);
                    basketall.Add(basket);
                    returnValue = "Found";
                }
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "GetAllBaskets", returnValue, startTime, timer.Elapsed, success);
            }
            return basketall;

            //var server = GetServer();
            //var data = server.Keys();

            //foreach(string key in data)
            //{

            //    var basket = await GetBasketAsync(key);
            //    basketall.Add(basket);
            //}
        }

        //public IEnumerable<string> GetUsers()
        //{
        //    var server = GetServer();
        //    var data = server.Keys();

        //    return data?.Select(k => k.ToString());
        //}

        private async Task<BasketEntity> GetBasketAsync(string basketid)
        {
            var data = await _database.StringGetAsync(basketid);

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<BasketEntity>(data);
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

    }
}
