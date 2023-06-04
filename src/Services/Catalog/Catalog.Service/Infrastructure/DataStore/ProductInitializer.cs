using Catalog.API.Contracts;
using Catalog.API.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Infrastructure.DataStore;
using System.Linq;
using System.IO;
using SharedUtilities.Utilties;
using Microsoft.Extensions.Logging;

namespace catalog.service.Infrastructure.DataStore
{
    public class ProductInitializer
    {
        private static int _counter;
        private DataContext _context;
        private ILogger<ProductInitializer> _logger;

        public async Task InitializeDatabaseAsync(IServiceScope serviceScope)
        {
            // Get DataContext and Logger explicitly from DI container
            _context = serviceScope.ServiceProvider.GetService<DataContext>();
            _logger = serviceScope.ServiceProvider.GetService<ILogger<ProductInitializer>>();
            
            Guard.ForNullObject(_context, "DataContext not found in DI container");

            // Get ProductRepository from DI container
            var productRepository = serviceScope.ServiceProvider.GetService<IProductRepository>();

            Guard.ForNullObject(productRepository, "ProductRepository not found in DI container");

            var databaseCreated = _context.Database.EnsureCreated();

            // Determine if database has been seeded by checking for any data in the Products table
            if (!_context.Products.Any())
            {
                // make sure child tables are dropped and tables reseeded for identity values (pass dummy correlation token)
                await productRepository.ClearProductDatabase("clearingdatabase");

                // Seed lookup data
                await SeedData<Genre>("genres.csv");
                await SeedData<Medium>("mediums.csv");
                await SeedData<Status>("statuses.csv");
                await SeedData<Condition>("conditions.csv");
                await SeedData<Artist>("artists.csv");

                // Seed products
                await SeedProducts();
            }
        }

        //public async Task SeedData<T>(IEnumerable<T> data) where T : class
        public async Task SeedData<T>(string dataFile) where T : class

        {
            try
            {
                var currentDirectory = Environment.CurrentDirectory;

                // File path for lookup data
                var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", dataFile);
                               
                // Skip header row from CSV File
                var lines = File.ReadAllLines(filePath).Skip(1);

                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var item = Activator.CreateInstance<T>();

                    // Add validation to ensure Name property exists
                    Guard.ForNullObject(item.GetType().GetProperty("Name"), $"Name property doesn't exist in {typeof(T).Name}");

                    // Set name property  
                    item.GetType().GetProperty("Name").SetValue(item, values[0]);

                    // Add item to EF context
                    _context.Set<T>().Add(item);
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Todo: Add Logging
                    var errorMessage = $"Error seeding {typeof(T).Name} data {ex.Message}";
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage , ex);
                }
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding {typeof(T).Name} data {ex.Data}";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage, ex);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding {typeof(T).Name} data {ex.Message}";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage, ex);
            }
        }

        public async Task<List<Product>> SeedProducts()
        {
            List<Product> products = new List<Product>();
            int counter = 1;

            try
            {
                var currentDirectory = Environment.CurrentDirectory;

                // File path for Genres
                var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "products.csv");

                // Skip header row from CSV File
                var lines = File.ReadAllLines(filePath).Skip(1); //.Take(75);
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var product = new Product
                    {
                        ProductId = Guid.NewGuid(),
                        ParentalCaution = false,
                        ReleaseYear = values[3],
                        Medium = _context.Mediums.Single(g => g.Name == values[6]),
                        Single = values[1],
                        Upc = GenerateUpc(),
                        Title = values[2],
                        Genre = _context.Genres.Single(g => g.Name == values[4]),
                        Price = GeneratePrice(),
                        Artist = _context.Artists.Single(a => a.Name == values[0]),
                        Status = _context.Status.Single(s => s.Name == values[5]),
                        Condition = _context.Conditions.Single(c => c.Name == values[7]),
                        AlbumArtUrl = SetMediumGraphic(values[6]),
                        CreateDate = DateTime.Now,
                        IsActive = true
                    };
                    products.Add(product);
                    _context.Products.Add(product);
                    counter++;
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Todo: Add Logging
                    var errorMessage = $"Error seeding Product Catalog data on record {counter}: {ex.Message}";
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage, ex);
                }
            }

            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding Product Catalog data on record {counter}: {ex.Data}";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage, ex);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding Product Catalog data on record {counter}: {ex.Message}";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage, ex);
            }

            return products;
        }


        // Generate mock UPC code
        private static string GenerateUpc()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[20];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

            return new string(stringChars);
        }


        // Set parental caution flag
        private static bool SetCaution()
        {
            _counter++;

            if (_counter % 5 == 0)
                return true;

            return false;
        }

        // Set medium graphic
        private static string SetMediumGraphic(string medium)
        {
            string graphicName;
            switch (medium)
            {
                case "EightTrack":
                    graphicName = "eight-track.jpg";
                    break;
                case "CD":
                    graphicName = "cd.jpg";
                    break;
                case "CassetteTape":
                    graphicName = "cassette.jpg";
                    break;
                case "Album":
                    graphicName = "cassette.jpg";
                    break;
                default:
                    graphicName = "placeholder.png";
                    break;
            }
            return graphicName;
        }


        // generate number between $10 and $100 as decimal rounding to nearest dollar
        private static decimal GeneratePrice()
        {
            Random random = new Random();
            decimal randomPrice = (decimal)random.Next(1000, 10000) / 100;
            return Math.Round(randomPrice);

            //Random rand = new Random();
            //int price = rand.Next(10, 99);
            //string formattedPrice = price + ".00";
            //return decimal.Parse(formattedPrice);

            //    var rnd = new Random();
            //    return (decimal)(String.Format("{0:C}", rnd.Next())).toDecimal();
            //   // return rnd.Next(1, 20);
        }
    }
}
