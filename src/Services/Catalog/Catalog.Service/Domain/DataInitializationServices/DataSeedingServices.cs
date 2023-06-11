using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using catalog.service.Contracts;
using catalog.service.Infrastructure.DataStore;
using catalog.service.Contracts;
using StackExchange.Redis;

namespace catalog.service.Domain.DataInitializationServices
{
    public class DataSeedingServices : IDataSeedingServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DataContext _dataContext;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DataSeedingServices> _logger;
        private readonly IDistributedCacheRepository _distributedCacheRepository;
        private readonly ConnectionMultiplexer _redisCache;

        public DataSeedingServices(DataContext dataContext,
                        IWebHostEnvironment webHostEnvironment,
                        IProductRepository productRepository,
                        ILogger<DataSeedingServices> logger,
                        IDistributedCacheRepository distributedCacheRepository,
                        ConnectionMultiplexer redis)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
            _logger = logger;
            _distributedCacheRepository = distributedCacheRepository;
            _redisCache = redis;
        }

        public async Task SeedDatabase(string correlationToken)
        {
            try
            {
                new ProductDatabaseInitializer(_dataContext,
                                               _webHostEnvironment,
                                               _distributedCacheRepository,
                                               _redisCache).InitializeDatabaseAsync().Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not run Product Initializer: {ex.Message}");
                throw;
            }
        }
    

    }
}
