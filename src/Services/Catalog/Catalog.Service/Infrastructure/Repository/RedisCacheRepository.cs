using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Linq;
using catalog.service.Contracts;
using System;

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
