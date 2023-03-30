using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tools.Database;
using Tools.Entities;
using Tools.TableStorage;

namespace Tools.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/ConnectionManager")]
    public class ConnectionManagerController : Controller
    {
        private const string ProductPartitionKey = "MusicProduct";
        private readonly IMusicRepository _musicRepository;
        private readonly IConfiguration _configuration;
        private readonly IBaseRespository<ProductEntity> _productRepository;

        public ConnectionManagerController(IMusicRepository musicRepository,
            IBaseRespository<ProductEntity> productRepository,
            IConfiguration configuration)
        {
            _musicRepository = musicRepository;
            _productRepository = productRepository;
            _configuration = configuration;
        }

        /// <summary>
        ///     Checks for Azure Resource Connectivity Errors 
        /// </summary>
        [HttpGet("CheckConnections", Name = "CheckConnectionsRoute")]
        public IActionResult CheckConnections()
        {
            var configurationState = string.Empty;

            var correlationToken = Guid.NewGuid().ToString();

            //// Can I read UserSecrets and present configuration data
            //configurationState = string.IsNullOrEmpty(_configuration["ServiceBusPublisherConnectionString"]) ? "ServiceBus Connection string missing/n" : string.Empty;


            //var serviceBusConnectionString = _configuration["ServiceBusPublisherConnectionString"];
            //var topicName = _configuration["ServiceBusTopicName"];
            //var subscriptionName = _configuration["ServiceBusSubscriptionName"];
            //var storageAccount = _configuration["StorageAccount"];
            //var storageKey = _configuration["StorageKey"];
            //var storageTable = _configuration["StorageTableName_Basket"];

            //var cosmosConnectionString = _configuration["CosmosEndpoint"];
            //var cosmosKey = _configuration["CosmosPrimaryKey"];
            //var databaseConnectonString = _configuration["CatalogConnectionString"];


            //var connectionString = configuration["ServiceBusPublisherConnectionString"];
            //var topicName = configuration["ServiceBusTopicName"];
            //var subscriptionName = configuration["ServiceBusSubscriptionName"];

            //Guard.ForNullOrEmpty(connectionString, "ConnectionString from Catalog is Null");
            //Guard.ForNullOrEmpty(topicName, "TopicName from Catalog is Null");
            //Guard.ForNullOrEmpty(subscriptionName, "SubscriptionName from Catalog is Null");

            // Can I connect to the SqlDB and query products?

            ////var products = _musicRepository.GetAll(correlationToken);

            ////if (products.Count < 1)
            ////    throw new Exception("Error in Seed Catalog Read Table -- Cannot get reference to the Products Table");

            ////var productEntityObjects = products.Select(x => new ProductEntity
            ////{
            ////    PartitionKey = ProductPartitionKey,
            ////    RowKey = x.Id.ToString(),
            ////    Title = x.Title,
            ////    ArtistName = x.Artist.Name,
            ////    Cutout = x.Cutout,
            ////    GenreName = x.Genre.Name,
            ////    ParentalCaution = x.ParentalCaution,
            ////    Price = x.Price.ToString(),
            ////    ReleaseDate = x.ReleaseDate,
            ////    Upc = x.Upc
            ////});

            ////try
            ////{
            ////    var currentReadTableItems = _productRepository.GetList(ProductPartitionKey).Result;
            ////    var count = currentReadTableItems.Count();

            ////    // Empty product read table
            ////    for (var i = 0; i < count; i++)
            ////        _productRepository.Delete(currentReadTableItems[i].PartitionKey, currentReadTableItems[i].RowKey);

            ////    // Populate product read table
            ////    foreach (var item in productEntityObjects) _productRepository.Insert(item, correlationToken);
            ////}
            ////catch (Exception ex)
            ////{
            ////    throw new Exception($"Could not build Catalog Read Table in DataInitializer. Message : {ex.Message}");
            ////}




            // Can I connect to Azure Storage and query on baskets?



            // Can I connect to Azure Service Bus and ensure that each Topic and subscription is present?



            // Can I connect to CosmosDB and query for orders?

            configurationState = "This feature is not enabled\n\n";

            return Ok(configurationState);
        }
    }
}