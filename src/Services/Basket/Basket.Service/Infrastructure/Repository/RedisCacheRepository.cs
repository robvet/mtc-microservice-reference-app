using Basket.Service.Contracts;
using Basket.Service.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Basket.Service.Domain.Entities;
using Microsoft.ApplicationInsights;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

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

        public async Task<bool> DeleteAsync<T>(Guid id, TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                return await _database.KeyDeleteAsync(id.ToString());
                success = true;
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency(typeof(T).Name, "Delete", success.ToString(), startTime, timer.Elapsed, success);
            }
        }

        //public async Task<bool> DeleteBasketAsync(Guid id, TelemetryClient telemetryClient)
        //{
        //    // Telemetry variables
        //    var success = false;
        //    var startTime = DateTime.UtcNow;
        //    var timer = System.Diagnostics.Stopwatch.StartNew();
            
        //    try
        //    {
        //        return await _database.KeyDeleteAsync(id);
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        telemetryClient.TrackException(ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        telemetryClient.TrackDependency("RedisCache", "DeleteBasket", success.ToString(), startTime, timer.Elapsed, success);
        //    }

        //    return success;
        //}



        public async Task<T> GetAsync<T>(Guid id,
                                 TelemetryClient telemetryClient,
                                 string methodName,
                                 string correlationToken)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue = "Not Found";

            try
            {
                var data = await _database.StringGetAsync(id.ToString());

                if (data.IsNullOrEmpty)
                {
                    return default(T);
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<T>(data);
                    success = true;
                    returnValue = "Found";
                    return result;
                }
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", methodName, returnValue, startTime, timer.Elapsed, success);
            }
        }

        //public async Task<BasketEntity> GetBasketAsync(string basketid,
        //                                               string correlationToken,
        //                                               TelemetryClient telemetryClient)
        //{
        //    // Telemetry variables
        //    var success = false;
        //    var startTime = DateTime.UtcNow;
        //    var timer = System.Diagnostics.Stopwatch.StartNew();
        //    var returnValue = "Not Found";

        //    try
        //    {
        //        var data = await _database.StringGetAsync(basketid);

        //        if (data.IsNullOrEmpty)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            var basketdata = JsonConvert.DeserializeObject<BasketEntity>(data);
        //            success = true;
        //            returnValue = "Found";
        //            return basketdata;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        telemetryClient.TrackException(ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        telemetryClient.TrackDependency("RedisCache", "GetBasket", returnValue, startTime, timer.Elapsed, success);
        //    }

        //    //var data = await _database.StringGetAsync(basketid);
        //    //if (data.IsNullOrEmpty)
        //    //{
        //    //    return null;
        //    //}
        //    //var basketdata = JsonConvert.DeserializeObject<BasketEntity>(data);
        //    //return basketdata;
        //}

        public async Task<bool> UpdateAsync<T>(T entity, Guid id, string correlationToken, TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue =  string.Empty;

            try
            {
                var created =
                    await _database.StringSetAsync(id.ToString(), JsonConvert.SerializeObject(entity));

                // _database.SetAdd()

                if (!created)
                {
                    _logger.LogInformation("Redis cache could not persist an item.");
                    returnValue = "Not Updated";
                    return false;
                }
                else
                {
                    success = true;
                    returnValue = "Updated Successfully";
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
                telemetryClient.TrackDependency("RedisCache","UpdateAsync", returnValue, startTime, timer.Elapsed, success);
            }

            return true;
        }


        //public async Task<BasketEntity> UpdateBasketAsync(BasketEntity basketEntity,
        //                                                  string correlationToken,
        //                                                  TelemetryClient telemetryClient)
        //{
        //    // Telemetry variables
        //    var success = false;
        //    var startTime = DateTime.UtcNow;
        //    var timer = System.Diagnostics.Stopwatch.StartNew();
        //    var returnValue = "Not Found";

        //    try
        //    {
        //        var created =
        //            await _database.StringSetAsync(basketEntity.BasketId, JsonConvert.SerializeObject(basketEntity));

        //        // _database.SetAdd()

        //        if (!created)
        //        {
        //            _logger.LogInformation("Redis cache could not persist an item.");
        //            return null;
        //        }
        //        else
        //        {
        //            success = true;
        //            returnValue = basketEntity.BasketId;
        //            _logger.LogInformation("Redis cache persisted item succesfully.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        telemetryClient.TrackException(ex);
        //        _logger.LogError(ex.ToString());
        //        throw;
        //    }
        //    finally
        //    {
        //        telemetryClient.TrackDependency("RedisCache", "UpdateBasket", returnValue, startTime, timer.Elapsed, success);
        //    }
            
        //    return await  GetBasketAsync(returnValue);
        //    //return null;
        //}

        public async Task<List<T>> GetAll<T>(string correlationToken, TelemetryClient telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue = "Not Found";

            var allItems = new List<T>();

            try
            {
                var server = GetServer();
                var data = server.Keys();

                foreach (string key in data)
                {
                    if (Guid.TryParse(key, out Guid guid))
                    {
                        var item = await GetAsync<T>(guid, telemetryClient, "GetAll", correlationToken);
                        allItems.Add(item);
                        returnValue = "Found";
                    }
                    //var item = await GetAsync<T>(key, telemetryClient, "GetAll", correlationToken);
                    //allItems.Add(item);
                    //returnValue = "Found";
                }

                success = true;
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }
            finally
            {
                telemetryClient.TrackDependency("RedisCache", "GetAll", returnValue, startTime, timer.Elapsed, success);
            }

            return allItems;
        }


        //public async Task<List<BasketEntity>> GetAllBaskets(string correlationToken,
        //                                                     TelemetryClient telemetryClient)
        //{
        //    // Telemetry variables
        //    var success = false;
        //    var startTime = DateTime.UtcNow;
        //    var timer = System.Diagnostics.Stopwatch.StartNew();
        //    var returnValue = "Not Found";

        //    var basketall = new List<BasketEntity>();

        //    try
        //    {
        //        var server = GetServer();
        //        var data = server.Keys();

        //        foreach (string key in data)
        //        {

        //            var basket = await GetBasketAsync(key);
        //            basketall.Add(basket);
        //            returnValue = "Found";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        telemetryClient.TrackException(ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        telemetryClient.TrackDependency("RedisCache", "GetAllBaskets", returnValue, startTime, timer.Elapsed, success);
        //    }
        //    return basketall;

        //    //var server = GetServer();
        //    //var data = server.Keys();

        //    //foreach(string key in data)
        //    //{

        //    //    var basket = await GetBasketAsync(key);
        //    //    basketall.Add(basket);
        //    //}
        //}

        //public IEnumerable<string> GetUsers()
        //{
        //    var server = GetServer();
        //    var data = server.Keys();

        //    return data?.Select(k => k.ToString());
        //}

        private async Task<T> GetAsync<T>(Guid id)
        {
            var data = await _database.StringGetAsync(id.ToString());

            if (data.IsNullOrEmpty)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(data);
        }

        private async Task<BasketEntity> GetBasketAsync(Guid id)
        {
            var data = await _database.StringGetAsync(id.ToString());

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
