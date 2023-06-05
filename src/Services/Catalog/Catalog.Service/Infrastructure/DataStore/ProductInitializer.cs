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
using Microsoft.IdentityModel.Tokens;

namespace catalog.service.Infrastructure.DataStore
{
    public class ProductInitializer
    {
        private static int _counter;
        private DataContext _context;
        private ILogger<ProductInitializer> _logger;
        const string GENRE = "genres.csv";
        const string MEDIUM = "mediums.csv";
        const string STATUS = "statuses.csv";
        const string CONDITION = "conditions.csv";
        const string ARTIST = "artists.csv";
        const string PRODUCT = "products2.csv";

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
                await SeedData<Genre>(GENRE);
                await SeedData<Medium>(MEDIUM);
                await SeedData<Status>(STATUS);
                await SeedData<Condition>(CONDITION);
                await SeedData<Artist>(ARTIST);

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
            Product product = null;
            string[] values;

            try
            {
                var currentDirectory = Environment.CurrentDirectory;

                // File path for Genres
                var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", PRODUCT);

                // Skip header row from CSV File
                var lines = File.ReadAllLines(filePath).Skip(1); //.Take(75);
                foreach (var line in lines)
                {
                    values = null;
                    values = line.Split(',');
                    product = new Product
                    {
                        ProductId = Guid.NewGuid(),

                        ParentalCaution = SetParentalCaution(),

                        // Inline validation for missing 'ReleaseYear' value
                        ReleaseYear = values[3].IsNullOrEmpty() ? "n/a" : values[3],
                        
                        // Inline validation to ensure Medium lookup value exist - Use SingleOrDefault to return null if not found
                        Medium = _context.Mediums.SingleOrDefault(g => g.Name == values[6]) ?? throw new Exception($"Missing lookup value from 'Medium' {values[6]} on record {counter}"),

                        // Inline validation for Missing 'Single' value (which would be the hit single from the album)
                        Single = values[1].IsNullOrEmpty() ? "n/a" : values[1],

                        Upc = GenerateUpc(),

                        // Inline validation for Missing 'Title' value
                        Title = !(values[2].IsNullOrEmpty()) ? values[2] : throw new Exception($"Missing 'ProductName' value on record {counter}"),

                        Genre = _context.Genres.SingleOrDefault(g => g.Name == values[4]) ?? throw new Exception($"Missing lookup value from 'Genre' {values[4]} on record {counter}"),

                        // Round price to 2 decimal places
                        Price = GeneratePrice(),

                        
                                                
                        Artist = _context.Artists.SingleOrDefault (a => a.Name == values[0]) ?? throw new Exception($"Missing lookup value from 'Artits' {values[0]} on record {counter}"),
                        Status = _context.Status.SingleOrDefault(s => s.Name == values[5]) ?? throw new Exception($"Missing lookup value from 'Status' {values[5]} on record {counter}"),
                        Condition = _context.Conditions.SingleOrDefault (c => c.Name == values[7]) ?? throw new Exception($"Missing lookup value from 'Condition' {values[7]} on record {counter}"),
                        CreateDate = DateTime.Now,
                        IsActive = true
                    };

                    // Round cost to 2 decimal places
                    product.Cost = GenerateCost(product.Price);
                    product.AlbumArtUrl = SetMediumGraphic(product.Medium.Name);
                    
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

        // generate number between $10 and $100 as decimal rounding to nearest dollar
        private static decimal GeneratePrice()
        {
            Random random = new Random();
            // Generate random price between $30 and $100
            decimal randomPrice = Math.Round((decimal)(random.Next(3000, 10000)) / 100, 2);
            return randomPrice;
        }

        // Generate random cost that is 30-70% of the price
        private decimal GenerateCost(decimal price)
        {
            Random random = new Random();
            // Generate random markup between 30% and 70%
            decimal markup = Convert.ToDecimal(random.Next(30, 71)) / 100m;   // Explicitly convert into decimal type.
            return Math.Round((decimal)(price * markup), 2);
        }


        // Generate mock UPC code
        private static string GenerateUpc()
        {
            // Generate random 12 digit UPC number
            var chars = "0123456789";
            var stringChars = new char[12];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

            return new string(stringChars);
        }

        // Set parental caution flag
        private static bool SetParentalCaution()
        {
            // Set ParentalCaution flag to true every 5th record
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
                    graphicName = "album.jpg";
                    break;
                default:
                    graphicName = "album.jpg";
                    break;
            }
            return graphicName;
        }


  
    }
}
