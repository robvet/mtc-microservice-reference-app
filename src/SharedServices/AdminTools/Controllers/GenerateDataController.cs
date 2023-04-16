using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminTools.Database;
using AdminTools.Entities;
using AdminTools.TableStorage;
using Microsoft.AspNetCore.Mvc;

namespace AdminTools.Controllers
{
    [Route("api/GenerateData")]
    public class GenerateDataController : Controller
    {

        public delegate Task BatchOperation(List<ProductEntity> items, string correlationToken);

        private const string ProductPartitionKey = "MusicProduct";
        private readonly IMusicRepository _musicRepository;
        private readonly IBaseRespository<ProductEntity> _productRepository;

        public GenerateDataController(IMusicRepository musicRepository,
            IBaseRespository<ProductEntity> productRepository)
        {
            _musicRepository = musicRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        ///     Genreates Catalog Read Table
        /// </summary>
        [HttpGet("GenerateCatalogReadTableData", Name = "GenerateTableDataRoute")]
        public async Task<IActionResult> GenerateCatalogReadTableData()
        {
            try
            {

                var correlationToken = Guid.NewGuid().ToString();

                // First, clear out any data in ReadModel
                var currentReadTableItems = _productRepository.GetList(ProductPartitionKey).Result;

                if (currentReadTableItems.Count() > 0)
                {
                    // Delete all product readmodel entities as a batch
                    await ProcessReadTable(currentReadTableItems, correlationToken, DeletetProduct);
                }

                var products = _musicRepository.GetAll(correlationToken);

                if (products.Count < 1)
                    throw new Exception("Error in Seed Catalog Read Table -- Cannot get reference to the Products Table");

                // Transform to InventoryItemEntity objects
                var productEntityObjects = products.Select(x => new ProductEntity
                {
                    PartitionKey = ProductPartitionKey,
                    RowKey = x.Id.ToString(),
                    Title = x.Title,
                    ArtistName = x.Artist.Name,
                    Cutout = x.Cutout,
                    GenreName = x.Genre.Name,
                    ParentalCaution = x.ParentalCaution,
                    Price = x.Price.ToString(),
                    ReleaseDate = x.ReleaseDate,
                    Upc = x.Upc
                }).ToList();

                // Add product readmodel entities as a batch
                await ProcessReadTable(productEntityObjects, correlationToken, InsertProduct);

            }
            catch (Exception ex)
            {
                throw new Exception($"Could not build Catalog Read Table in DataInitializer. Message : {ex.Message}");
            }


            return NoContent();
        }

        /// <summary>
        ///     Generates Catalog Database.
        /// </summary>
        [HttpGet("GenerateCatalogDatabaseData", Name = "GenerateDatabaseDataRoute")]
        public IActionResult GenerateCatalogDatabaseData()
        {
            throw new NotImplementedException("Generate Catalog Database has not been implemented");
        }

        /// <summary>
        /// Generic method that can either insert or delete all products as a batch
        /// </summary>
        /// <returns></returns>
        private async Task ProcessReadTable(List<ProductEntity> productEntityObjects, string correlationToken, BatchOperation operation)
        {
            var throlledProductEntityObjects = new List<ProductEntity>();

            int totalCount = productEntityObjects.Count - 1;
            int currentCount = 1;

            for (var i = 0; i <= totalCount; i++)
            {
                // Throttle product input - Azure Table Storage has a limit of 100 items per call
                throlledProductEntityObjects.Add(productEntityObjects[i]);

                if (currentCount > 99)
                {
                    try
                    {
                        await operation(throlledProductEntityObjects, correlationToken);
                        //await _productRepository.Insert(throlledProductEntityObjects, correlationToken);
                        currentCount = 1;
                        throlledProductEntityObjects.Clear();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                else
                    currentCount++;
            }

            // This sweep picks up any remaining products
            await operation(throlledProductEntityObjects, correlationToken);
            //await _productRepository.Insert(throlledProductEntityObjects, correlationToken);

        }

        private async Task InsertProduct(List<ProductEntity> throlledProductEntityObjects, string correlationToken)
        {
            await _productRepository.Insert(throlledProductEntityObjects, correlationToken);
        }

        private async Task DeletetProduct(List<ProductEntity> throlledProductEntityObjects, string correlationToken)
        {
            await _productRepository.Delete(throlledProductEntityObjects, correlationToken);
        }
    }
}