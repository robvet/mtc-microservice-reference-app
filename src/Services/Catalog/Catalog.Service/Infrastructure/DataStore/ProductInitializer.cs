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
                    // make sure child tables are dropped and tables reseeded for identity values
                    await productRepository.ClearProductDatabase("abc");

                    // Seed database
                    //await Seed(context);

                    var currentDirectory = Environment.CurrentDirectory;
                    var filePath = Path.Combine(currentDirectory, "Infrastructure", "SeedData", "products.csv");

                    //var filePath = "path/to/your/csv/file";
                    var genres = new List<Genre> { new Genre { Name = "Southern Rock" } };
                    var artists = new List<Artist> { new Artist { Name = "The Black Crowes" } };

                    var products = ReadProductsFromCsv(filePath, genres, artists);


                }
            }
        }

        // Assuming the CSV file has columns: Title, Genre, Artist, Price, AlbumArtUrl 
        public List<Product> ReadProductsFromCsv(string filePath, List<Genre> genres, List<Artist> artists)
        {
            List<Product> products = new List<Product>();

            try
            {
                var lines = File.ReadAllLines(filePath).Skip(1);
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    var product = new Product
                    {
                        Cutout = false,
                        ProductId = Guid.NewGuid(),
                        ParentalCaution = false,
                        ReleaseDate = SetReleaseDate(),
                        Upc = GenerateUpc(),
                        Title = values[2],
                        Genre = genres.Single(g => g.Name == values[4]),
                        //Price = decimal.Parse(values[3]),
                        Artist = artists.Single(a => a.Name == values[0]),
                        AlbumArtUrl = values[8]
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
    

        private static async Task Seed(Catalog.API.Infrastructure.DataStore.DataContext context)
        {
            //var users = new List<User>
            //{
            //    new User {Name = "Admin", Password = "musicstore", EmailAddress = "admin@musicstore.com"},
            //    new User {Name = "Customer", Password = "musicstore", EmailAddress = "customer@musicstore.com"},
            //}
            //; //.ForEach(x => context.Users.Add(x));

            ////new List<User>
            ////{
            ////    new User {Name = "Admin", Password = "musicstore", EmailAddress = "admin@musicstore.com"},
            ////    new User {Name = "John", Password = "musicstore", EmailAddress = "johnsmith@musicstore.com"},
            ////}.ForEach(x => context.Users.Add(x));

            //var roles = new List<Role>
            //{
            //    new Role {Name = "Administrator", Description = "Administrator",},
            //    new Role {Name = "Customer", Description = "Customer",},
            //}; //.ForEach(x => context.Roles.Add(x));

            ////new List<Role>
            ////{
            ////    new Role {Name = "Admin", Description = "Admin",},
            ////    new Role {Name = "Customer", Description = "Customer",},
            ////}.ForEach(x => context.Roles.Add(x));


            //new List<UserRole>
            //{
            //    new UserRole
            //    {
            //        User = users.Single(x => x.Name == "Admin"), 
            //        Role = roles.Single(x => x.Name =="Administrator"),
            //    },
            //    new UserRole
            //    {
            //        User = users.Single(x => x.Name == "Customer"), 
            //        Role = roles.Single(x => x.Name =="Customer"),
            //    },
            //}.ForEach(x => context.UserRoles.Add(x));

            var genres = new List<Genre>
            {
                new Genre {Name = "Rock"},
                new Genre {Name = "Jazz"},
                new Genre {Name = "Metal"},
                new Genre {Name = "Alternative"},
                new Genre {Name = "Disco"},
                new Genre {Name = "Blues"},
                new Genre {Name = "Latin"},
                new Genre {Name = "Reggae"},
                new Genre {Name = "Pop"},
                new Genre {Name = "Classical"},
                new Genre {Name = "Country"}
            };

            var artists = new List<Artist>
            {
                new Artist {Name = "Aerosmith"},
                new Artist {Name = "AC/DC"},
                new Artist {Name = "Animals"},
                new Artist {Name = "Allman Brothers"},

  
            };

            try
            {
                new List<Product>
                {
                    // Country

                    new Product
                    {
                        Cutout = false,
                        ProductId = Guid.NewGuid(),
                        ParentalCaution = false,
                        ReleaseDate = SetReleaseDate(),
                        Upc = GenerateUpc(),
                        Title = "Your Cheatin' Heart",
                        Genre = genres.Single(g => g.Name == "Country"),
                        Price = GenerateAlbumPrice(),
                        Artist = artists.Single(a => a.Name == "Hank Williams"),
                        AlbumArtUrl = "placeholder.png"
                    },
                    new Product
                    {
                        Cutout = false,
                        ProductId = Guid.NewGuid(),
                        ParentalCaution = false,
                        ReleaseDate = SetReleaseDate(),
                        Upc = GenerateUpc(),
                        Title = "He Stopped Loving Her Today",
                        Genre = genres.Single(g => g.Name == "Country"),
                        Price = GenerateAlbumPrice(),
                        Artist = artists.Single(a => a.Name == "George Jones"),
                        AlbumArtUrl = "placeholder.png"
                    },
                                   }.ForEach(a => context.Products.Add(a));

                try
                {
                    context.SaveChanges();
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
        }

        private static string GenerateUpc()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

            return new string(stringChars);
        }

        private static bool SetCaution()
        {
            _counter++;

            if (_counter % 5 == 0)
                return true;

            return false;
        }

        private static DateTime SetReleaseDate()
        {
            var gen = new Random();
            var start = new DateTime(1995, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        // generate number between $10 and $100 as decimal rounding to nearest dollar
        private static decimal GenerateAlbumPrice()
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
