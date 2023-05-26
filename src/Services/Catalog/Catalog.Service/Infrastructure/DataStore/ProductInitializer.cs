using Catalog.API.Contracts;
using Catalog.API.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Infrastructure.DataStore;
using System.Linq;
using System.IO;

namespace catalog.service.Infrastructure.DataStore
{
    public class ProductInitializer
    {
        private static int _counter;
        private static IProductRepository _ProductRepository;
        private DataContext _context;

        public async Task InitializeDatabaseAsync(IServiceScope serviceScope)
        {
            //var context = serviceScope.ServiceProvider.GetService<DataContext>();
            _context = serviceScope.ServiceProvider.GetService<DataContext>();
            var productRepository = serviceScope.ServiceProvider.GetService<IProductRepository>();

            if (_context != null)
            {
                var databaseCreated = _context.Database.EnsureCreated();

                // Determine if database has been seeded
                if (!_context.Products.Any())
                {
                    // make sure child tables are dropped and tables reseeded for identity values (pass dummy correlation token)
                    await productRepository.ClearProductDatabase("clearingdatabase");

                    // Seed database
                    //await Seed(context);

                    await SeedGenres();
                    await SeedMediums();
                    await SeedStatuses();
                    await SeedConditions();
                    await SeedArtists();
                    await SeedProducts();

                    //var currentDirectory = Environment.CurrentDirectory;
                    //var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "products.csv");

                    ////var filePath = "path/to/your/csv/file";
                    //var genres = new List<Genre> { new Genre { Name = "Southern Rock" } };
                    //var artists = new List<Artist> { new Artist { Name = "The Black Crowes" } };

                    //var products = ReadProductsFromCsv(filePath, genres, artists);
                }
            }
        }

        public async Task SeedGenres()
        {
            try
            {
                var currentDirectory = Environment.CurrentDirectory;

                // File path for Genres
                var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "genres.csv");

                ////var filePath = "path/to/your/csv/file";
                //var genres = new List<Genre> { new Genre { Name = "Southern Rock" } };
                //var artists = new List<Artist> { new Artist { Name = "The Black Crowes" } };

                var lines = File.ReadAllLines(filePath).Skip(1);
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var genre = new Genre
                    {
                        Name = values[0]
                    };
                    _context.Genres.Add(genre);
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Todo: Add Logging
                    var errorMessage = $"Error seeding Genre data {ex.Message}";
                    throw;
                }
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding Genre data {ex.Data}";
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding Genre data {ex.Message}";
                throw;
            }
        }

        public async Task SeedMediums()
        {
            try
            {
                var currentDirectory = Environment.CurrentDirectory;

                // File path for Genres
                var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "mediums.csv");

                var lines = File.ReadAllLines(filePath).Skip(1);
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var mediums = new Medium
                    {
                        Name = values[0]
                    };
                    _context.Mediums.Add(mediums);
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Todo: Add Logging
                    var errorMessage = $"Error seeding Medium data {ex.Message}";
                    throw;
                }
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding Medium data {ex.Data}";
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding Medium data {ex.Message}";
                throw;
            }
        }

        public async Task SeedStatuses()
        {
            try
            {
                var currentDirectory = Environment.CurrentDirectory;

                // File path for Genres
                var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "statuses.csv");

                var lines = File.ReadAllLines(filePath).Skip(1);
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var statuses = new Status
                    {
                        Name = values[0]
                    };
                    _context.Status.Add(statuses);
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Todo: Add Logging
                    var errorMessage = $"Error seeding Status data {ex.Message}";
                    throw;
                }
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding Status data {ex.Data}";
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding Status data {ex.Message}";
                throw;
            }
        }


        public async Task SeedConditions()
        {
            try
            {
                var currentDirectory = Environment.CurrentDirectory;

                // File path for Genres
                var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "conditions.csv");

                var lines = File.ReadAllLines(filePath).Skip(1);
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var conditions = new Condition
                    {
                        Name = values[0]
                    };
                    _context.Conditions.Add(conditions);
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Todo: Add Logging
                    var errorMessage = $"Error seeding Condition data {ex.Message}";
                    throw;
                }
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding Condition data {ex.Data}";
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding Condition data {ex.Message}";
                throw;
            }
        }

        public async Task SeedArtists()
        {
            try
            {
                var currentDirectory = Environment.CurrentDirectory;

                // File path for Genres
                var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "artists.csv");

                var lines = File.ReadAllLines(filePath).Skip(1);
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var artists = new Artist
                    {
                        Name = values[0]
                    };
                    _context.Artists.Add(artists);
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Todo: Add Logging
                    var errorMessage = $"Error seeding Artist data {ex.Message}";
                    throw;
                }
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding Artist data {ex.Data}";
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding Artist data {ex.Message}";
                throw;
            }
        }

        public async Task<List<Product>> SeedProducts()
        {
            List<Product> products = new List<Product>();

                try
                {
                    var currentDirectory = Environment.CurrentDirectory;

                    // File path for Genres
                    var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "products.csv");

                    ////var filePath = "path/to/your/csv/file";
                    //var genres = new List<Genre> { new Genre { Name = "Southern Rock" } };
                    //var artists = new List<Artist> { new Artist { Name = "The Black Crowes" } };

 
                var lines = File.ReadAllLines(filePath).Skip(1);
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
                        AlbumArtUrl = values[8],
                        CreateDate = DateTime.Now,
                        IsActive = true
                    };
                    products.Add(product);
                    _context.Products.Add(product);
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Todo: Add Logging
                    var errorMessage = $"Error seeding Product Catalog data {ex.Message}";
                    throw;
                }
            }

            catch (InvalidOperationException ex)
            {
                var errorMessage = $"Error seeding Product Catalog data {ex.Data}";
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error seeding Product Catalog data {ex.Message}";
                throw;
            }

            return products;
        }

      

        //private static async Task SeedProducts()
        //{
        //    //var users = new List<User>
        //    //{
        //    //    new User {Name = "Admin", Password = "musicstore", EmailAddress = "admin@musicstore.com"},
        //    //    new User {Name = "Customer", Password = "musicstore", EmailAddress = "customer@musicstore.com"},
        //    //}
        //    //; //.ForEach(x => context.Users.Add(x));

        //    ////new List<User>
        //    ////{
        //    ////    new User {Name = "Admin", Password = "musicstore", EmailAddress = "admin@musicstore.com"},
        //    ////    new User {Name = "John", Password = "musicstore", EmailAddress = "johnsmith@musicstore.com"},
        //    ////}.ForEach(x => context.Users.Add(x));

        //    //var roles = new List<Role>
        //    //{
        //    //    new Role {Name = "Administrator", Description = "Administrator",},
        //    //    new Role {Name = "Customer", Description = "Customer",},
        //    //}; //.ForEach(x => context.Roles.Add(x));

        //    ////new List<Role>
        //    ////{
        //    ////    new Role {Name = "Admin", Description = "Admin",},
        //    ////    new Role {Name = "Customer", Description = "Customer",},
        //    ////}.ForEach(x => context.Roles.Add(x));


        //    //new List<UserRole>
        //    //{
        //    //    new UserRole
        //    //    {
        //    //        User = users.Single(x => x.Name == "Admin"), 
        //    //        Role = roles.Single(x => x.Name =="Administrator"),
        //    //    },
        //    //    new UserRole
        //    //    {
        //    //        User = users.Single(x => x.Name == "Customer"), 
        //    //        Role = roles.Single(x => x.Name =="Customer"),
        //    //    },
        //    //}.ForEach(x => context.UserRoles.Add(x));

        //    var genres = new List<Genre>
        //    {
        //        new Genre {Name = "Rock"},
        //        new Genre {Name = "Jazz"},
        //        new Genre {Name = "Metal"},
        //        new Genre {Name = "Alternative"},
        //        new Genre {Name = "Disco"},
        //        new Genre {Name = "Blues"},
        //        new Genre {Name = "Latin"},
        //        new Genre {Name = "Reggae"},
        //        new Genre {Name = "Pop"},
        //        new Genre {Name = "Classical"},
        //        new Genre {Name = "Country"}
        //    };

        //    var artists = new List<Artist>
        //    {
        //        new Artist {Name = "Aerosmith"},
        //        new Artist {Name = "AC/DC"},
        //        new Artist {Name = "Animals"},
        //        new Artist {Name = "Allman Brothers"},

  
        //    };

        //    try
        //    {
        //        new List<Product>
        //        {
        //            // Country

        //            new Product
        //            {
        //                Cutout = false,
        //                ProductId = Guid.NewGuid(),
        //                ParentalCaution = false,
        //                ReleaseDate = SetReleaseDate(),
        //                Upc = GenerateUpc(),
        //                Title = "Your Cheatin' Heart",
        //                Genre = genres.Single(g => g.Name == "Country"),
        //                Price = GenerateAlbumPrice(),
        //                Artist = artists.Single(a => a.Name == "Hank Williams"),
        //                AlbumArtUrl = "placeholder.png"
        //            },
        //            new Product
        //            {
        //                Cutout = false,
        //                ProductId = Guid.NewGuid(),
        //                ParentalCaution = false,
        //                ReleaseDate = SetReleaseDate(),
        //                Upc = GenerateUpc(),
        //                Title = "He Stopped Loving Her Today",
        //                Genre = genres.Single(g => g.Name == "Country"),
        //                Price = GenerateAlbumPrice(),
        //                Artist = artists.Single(a => a.Name == "George Jones"),
        //                AlbumArtUrl = "placeholder.png"
        //            },
        //                           }.ForEach(a => _context.Products.Add(a));

        //        try
        //        {
        //            _context.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Todo: Add Logging
        //            var errorMessage = $"Error seeding Product Catalog data {ex.Message}";
        //            throw;
        //        }
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        var errorMessage = $"Error seeding Product Catalog data {ex.Data}";
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        var errorMessage = $"Error seeding Product Catalog data {ex.Message}";
        //        throw;
        //    }
        //}


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
