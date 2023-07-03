using System.Linq;
using System.IO;
using SharedUtilities.Utilties;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using catalog.service.Domain.Entities;
using catalog.service.Infrastructure.DataStore;
using catalog.service.Contracts;
using StackExchange.Redis;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace catalog.service.Domain.DataInitializationServices

{
    public class ProductDatabaseInitializer
    {
        private static int _counter;
        private readonly DataContext _context;
        private ILogger<ProductDatabaseInitializer> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ConnectionMultiplexer _redisMultiplexer;
        private IDatabase _redisDatabase;

        // Price generation
        private readonly int _lowPrice = 2500; // $25.00
        private readonly int _highPrice = 15000; // $150.00
        private readonly decimal _highValuePercentage = 0.75m; // 75%
        private readonly decimal highValuePrice; 
        private readonly bool _dropDatabase;


        //const string GENRE = "genres.csv";
        //const string MEDIUM = "mediums.csv";
        //const string STATUS = "statuses.csv";
        //const string CONDITION = "conditions.csv";
        //const string ARTIST = "artists.csv";

        const string GENRE = "genre.csv";
        const string MEDIUM = "medium.csv";
        const string STATUS = "status.csv";
        const string CONDITION = "condition.csv";
        const string ARTIST = "artist.csv";


        const string PRODUCT = "products2.csv";
        const string CONTENT_DIRECTORY = "Content";

        public ProductDatabaseInitializer(DataContext context, 
                                          IWebHostEnvironment webHostEnvironment,
                                          bool dropDatabase,
                                          ConnectionMultiplexer redis
                                          )
        {
            _context = context;

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger<ProductDatabaseInitializer>();

            _webHostEnvironment = webHostEnvironment;

            _redisMultiplexer = redis;

            highValuePrice = _highValuePercentage * (_highPrice / 100);

            _dropDatabase = dropDatabase;
        }

        public async Task InitializeDatabaseAsync()
        {
            Guard.ForNullObject(_context, "DataContext not found in DI container");
            Guard.ForNullObject(_logger, "Logger not instantiated for DataSeedingServices");
            Guard.ForNullObject(_webHostEnvironment, "WebHostEnvironment Null in DataSeedingServices");
            Guard.ForNullObject(_redisMultiplexer, "RedisMultiplexer Null in DataSeedingServices");

            var databaseCreated = _context.Database.EnsureCreated();

            if (databaseCreated)
            {
                _logger.LogInformation("Database has been created");
            }
            else
            {
                _logger.LogInformation("Database already exists");
            }

            await ClearData(_dropDatabase);

            // Ensure that all CSV files have lowercase names - if not, throw exception
            // Otherwise, SeedLookupData will fail 
            var contentRootPath = _webHostEnvironment.ContentRootPath;
            var filePaths = Directory.GetFiles(Path.Combine(contentRootPath, CONTENT_DIRECTORY));
            var fileNames = Directory.GetFiles(Path.Combine(contentRootPath, CONTENT_DIRECTORY)).Select(path => Path.GetFileName(path));
            var incorrectFileNames = new List<string>();    

            foreach (var file in fileNames)
            {
                var fileName = Path.GetFileName(file);
                var lowerCaseFileName = fileName.ToLower();

                if (fileName != lowerCaseFileName)
                {
                    incorrectFileNames.Add(fileName);
                }
            }

            if (incorrectFileNames.Count > 0)
            {
                var flattenedNames = string.Join(", ", incorrectFileNames);
                var fileNameErrors = $"The following files have uppercase naming {flattenedNames}. Please rename file to all lowercase. Caught in ProductDatabaseInitializer";
                _logger.LogError(fileNameErrors);
                throw new Exception(fileNameErrors);
            }

            // Seed lookup data
            await SeedLookupData<Artist>(ARTIST);
            await SeedLookupData<Genre>(GENRE);
            await SeedLookupData<Medium>(MEDIUM);
            await SeedLookupData<Status>(STATUS);
            await SeedLookupData<Entities.Condition>(CONDITION);
            
            // Seed dbProducts
            await SeedProductData();
        }

        private async Task SeedLookupData<T>(string lookupType) where T : class
        {
            try
            {
                //var currentDirectory = Environment.CurrentDirectory;

                //// File path for lookup data
                //var filePath = Path.Combine(currentDirectory, "Infrastructure", "DataSeedingServices", lookupType);

                // File path for lookup data
                var contentRootPath = _webHostEnvironment.ContentRootPath;

                _logger.LogInformation($"In SeedLookupData, contentRootPath = {contentRootPath}");

                // Force lowercase for lookupType
                var filePath = Path.Combine(contentRootPath, CONTENT_DIRECTORY, lookupType.ToLower());

                
                _logger.LogInformation("Content FilePath is {filePath}", filePath);

                IEnumerable<string> lines;

                try
                {
                    // Skip(1) skills first row which is header row from CSV File
                    lines = File.ReadAllLines(filePath).Skip(1);
                }
                catch (Exception ex)
                {
                    // You can use the String method "ToLower()" to convert the string value to lowercase
                    // If the resulting string is equal to the original string, then the original string was already lowercase.
                    var errorMessage = $"Error reading file {lookupType} in {filePath}: {ex.Message}; CHECK FOR CASE. FILE NAME MUST BE ALL LOWERCASE";
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage, ex);
                }

                _logger.LogInformation($"Found lookup file {lookupType}");

                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var item = Activator.CreateInstance<T>();

                    // Add validation to ensure Name property exists
                    Guard.ForNullObject(item.GetType().GetProperty("Name"), $"Name property doesn't exist in {typeof(T).Name}");
                    Guard.ForNullObject(item.GetType().GetProperty("GuidId"), $"Name property doesn't exist in {typeof(T).Name}");

                    // Set name property  
                    item.GetType().GetProperty("Name").SetValue(item, values[0]);
                    item.GetType().GetProperty("GuidId").SetValue(item, Guid.Parse(values[1]));

                    // Add item to EF context
                    _context.Set<T>().Add(item);
                }

                _logger.LogInformation($"Inserted {lookupType} lookup records into Products Database");

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Error seeding {lookupType} data in {filePath}: {ex.Message}";
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage, ex);
                }
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding {lookupType} data {ex.Data}";
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

        public async Task<List<Product>> SeedProductData()
        {
            List<Product> dbProducts = new();
            List<ProductReadModel> readModels = new();

            var keyValuePairs = new List<KeyValuePair<RedisKey, RedisValue>>();

            int counter = 0;
            Product product = null;
            ProductReadModel productReadModel = null;
            string[] itemValues;

            try
            {
                //var currentDirectory = Environment.CurrentDirectory;

                //// File path for Genres
                //var filePath = Path.Combine(currentDirectory, "Infrastructure", "DataSeedingServices", PRODUCT);

                var contentRootPath = _webHostEnvironment.ContentRootPath;
                var filePath = Path.Combine(contentRootPath, CONTENT_DIRECTORY, PRODUCT);

                // Skip header row from CSV File
                var items = File.ReadAllLines(filePath).Skip(1); //.Take(10);
                foreach (var item in items)
                {
                    itemValues = null;
                    itemValues = item.Split(',');
                    product = new Product
                    {
                        // Inline validation for missing 'ProductId' value
                        ProductId =  itemValues[9].IsNullOrEmpty() ? throw new Exception($"Missing value from 'ProductId' on record {counter}") : new Guid(itemValues[9]),

                        ParentalCaution = SetParentalCaution(),

                        // Inline validation for missing 'ReleaseYear' value
                        ReleaseYear = itemValues[3].IsNullOrEmpty() ? throw new Exception($"Missing value from 'Release Year' on record {counter}") : itemValues[3],

                        // Inline validation to ensure Medium lookup value exist -
                        // -- Use SingleOrDefault to return null if not found
                        // -- Use NoCommaValidation to remove any commas from the value, which would cause an error
                        // -- Use ?? to throw exception if null
                        Medium = _context.Mediums.SingleOrDefault(g => g.Name == NoCommaValidation(itemValues[6])) ?? throw new Exception($"Missing lookup value from 'Medium' {itemValues[6]} on record {counter}"),

                        Single = itemValues[1].IsNullOrEmpty() ? throw new Exception($"Missing value from 'Single' on record {counter}") : itemValues[1],

                        Upc = GenerateUpc(),

                        // Inline validation for Missing 'Title' value
                        Title = itemValues[2].IsNullOrEmpty() ? throw new Exception($"Missing value from 'Title' on record {counter}") : itemValues[2],

                        AlbumArtUrl = null, // SetMediumGraphic(product.Medium.Name);

                        Genre = _context.Genres.SingleOrDefault(g => g.Name == NoCommaValidation(itemValues[4])) ?? throw new Exception($"Missing lookup value from 'Genre' {itemValues[4]} on record {counter}"),

                        // Round price and cost to 2 decimal places
                        Price = GeneratePrice(_lowPrice, _highPrice),
                        
                        Artist = await _context.Artists.SingleOrDefaultAsync(a => a.Name == NoCommaValidation(itemValues[0])) ?? throw new Exception($"Missing lookup value from 'Artits' {itemValues[0]} on record {counter}"),
                        Status = await _context.Status.SingleOrDefaultAsync(s => s.Name == NoCommaValidation(itemValues[5])) ?? throw new Exception($"Missing lookup value from 'Status' {itemValues[5]} on record {counter}"),
                        Condition = await _context.Conditions.SingleOrDefaultAsync(c => c.Name == NoCommaValidation(itemValues[7])) ?? throw new Exception($"Missing lookup value from 'Condition' {itemValues[7]} on record {counter}"),
                        CreateDate = DateTime.Now,


                        IsActive = true
                    };

                    product.HighValueItem = SetHighValueItem(product.Price);
                    product.Cost = GenerateCost(product.Price);

                    dbProducts.Add(product);
                    _context.Products.Add(product);

                    // Add items to the read model
                    productReadModel = new ProductReadModel
                    {
                        ProductId = product.ProductId,
                        ArtistId = product.Artist.GuidId,
                        Artist = product.Artist.Name,
                        GenreId = product.Genre.GuidId,
                        Genre= product.Genre.Name,
                        MediumId = product.Medium.GuidId,
                        Medium = product.Medium.Name,
                        Status = product.Status.Name,
                        Condition = product.Condition.Name,
                        Title = product.Title,
                        Price = product.Price,
                        HighValueItem = product.HighValueItem
                    };

                    // Add items to the list
                    keyValuePairs.Add(new KeyValuePair<RedisKey,RedisValue>(product.ProductId.ToString(), JsonConvert.SerializeObject(productReadModel)));

                    counter++;
                }

                // Save changes to the database
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Error seeding Product Catalog data on record {counter}: {ex.Message}";
                    _logger.LogError(errorMessage, ex);
                    throw new Exception(errorMessage, ex);
                }

                // Instantiates the Redis Multiplexer and Database
                try
                {
                    _redisDatabase = _redisMultiplexer.GetDatabase();
                }
                catch (Exception ex)
                {
                    // Capture error, report it, but continue
                    var errorMessage = $"Error creating Redis Database from Redis Multiplexer in Product Seeding: {ex.Message}";
                    _logger.LogError(errorMessage, ex);
                }

                try
                {
                    // Delete all keys from the database
                    _redisDatabase.Execute("FLUSHDB");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error clearing Redis database");
                    throw;
                }

                try
                {
                    // Capture error, report it, but continue
                    await _redisDatabase.StringSetAsync(keyValuePairs.ToArray());
                }
                catch (Exception ex)
                {
                    // Capture error, report it, but continue
                    var errorMessage = $"Error assigning read model cache vaules in Product Seeding: {ex.Message}";
                    _logger.LogError(errorMessage, ex);
                }
            }

            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding Product Catalog data on record {counter}: {ex.Data}";
                _logger.LogError(errorMessage, ex);
                throw new Exception(errorMessage, ex);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding Product Catalog data on record {counter}: {ex.Message}";
                _logger.LogError(errorMessage, ex);
                throw new Exception(errorMessage, ex);
            }

            _logger.LogInformation($"Inserted {counter} dbProducts into Products Database");

            return dbProducts;
        }

        // Set HighValue flag for items that exceed predetermined high value price set in constructor
        private bool SetHighValueItem(decimal price)
        {
            if (price > highValuePrice)
                {
                return true;
            }
            else
            {
                return false;
            }
        }

        // generate number between $10 and $100 as decimal rounding to nearest dollar
        private static decimal GeneratePrice(int lowPrice, int highPrice)
        {
            Random random = new();
            // Generate random price between $30 and $200
            decimal randomPrice = Math.Round((decimal)random.Next(lowPrice, highPrice) / 100, 2);
            return randomPrice;
        }

        // Generate random cost that is 30-70% of the price
        private decimal GenerateCost(decimal price)
        {
            Random random = new Random();
            // Generate random markup between 30% and 70%
            decimal markup = Convert.ToDecimal(random.Next(30, 71)) / 100m;   // Explicitly convert into decimal type.
            return Math.Round(price * markup, 2);
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

        private static string NoCommaValidation(string cell)
        {
            if (cell.Contains(','))
            {
                return cell.Replace(",", "", StringComparison.OrdinalIgnoreCase);
            }
            return cell;
        }

        private async Task ClearData(bool dropDatabase)
        {
            
            try
            {
                // Delete data from all tables
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Products");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Artists");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Genres");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Conditions");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Mediums");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Descriptions");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Status");
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Could not Clear Data in BaseRepository : {ex.Message}");
            }
            catch (Exception ex)
            {
                //var traveredMessage = ExceptionHandlingUtilties.TraverseException(ex);
                //throw new Exception($"Could not Save in BaseRepository : {traveredMessage}");
                throw new Exception($"Could not Clear Data in BaseRepository : {ex.Message}");
            }
        

            if (dropDatabase)
            { 
                try
                {
                    // Delete data from all tables
                    await _context.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].Descriptions");
                    await _context.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].Products");
                    await _context.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].Genres");
                    await _context.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].Artists");
                    await _context.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].Conditions");
                    await _context.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].Mediums");
                    await _context.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].Status");
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception($"Could not Drop Databases : {ex.Message}");
                }
                catch (Exception ex)
                {
                    //var traveredMessage = ExceptionHandlingUtilties.TraverseException(ex);
                    //throw new Exception($"Could not Save in BaseRepository : {traveredMessage}");
                    throw new Exception($"Could not Drop Databases : {ex.Message}");
                }

                // Recreate database an tables
                try
                {
                    _context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could recreate databases : {ex.Message}");
                }
            }

            try
            {
                // Reset identity columns
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Products', RESEED, 1);");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Artists', RESEED, 1);");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Genres', RESEED, 1);");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Conditions', RESEED, 1);");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Mediums', RESEED, 1);");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Descriptions', RESEED, 1);");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Status', RESEED, 1);");
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Could not reset identity value in BaseRepository : {ex.Message}");
            }
            catch (Exception ex)
            {
                //var traveredMessage = ExceptionHandlingUtilties.TraverseException(ex);
                //throw new Exception($"Could not Save in BaseRepository : {traveredMessage}");
                throw new Exception($"Could not reset identity value in BaseRepository : {ex.Message}");
            }
        }
    }
}
