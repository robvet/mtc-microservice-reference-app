using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Catalog.API.Contracts;
using Catalog.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Infrastructure.DataStore
{
    public static class DataInitializer
    {
        private static int _counter;
        private static IProductRepository _ProductRepository;

        public static async Task InitializeDatabaseAsync(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            var productRepository = serviceScope.ServiceProvider.GetService<IProductRepository>();

            if (context != null)
            {
                var databaseCreated = context.Database.EnsureCreated();

                // Determine if database has been seeded
                if (!context.Products.Any())
                {
                    // make sure child tables are dropped and tables reseeded for identity values
                    //await productRepository.ClearProductDatabase("abc");
                    // Seed database
                    await Seed(context);
                }
            }
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

        private static async Task Seed(DataContext context)
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

                new Artist {Name = "Bad Company"},
                new Artist {Name = "Black Sabbath"},
                new Artist {Name = "Beatles"},
                new Artist {Name = "Bob Dylan"},
                new Artist {Name = "Buffalo Springfield"},
                new Artist {Name = "Bruce Springsteen"},
                new Artist {Name = "Blue Oyster Cult"},
                new Artist {Name = "Boston"},
                new Artist {Name = "Bob Seger"},
                new Artist {Name = "BTO"},

                new Artist {Name = "CCR"},
                new Artist {Name = "Cars"},
                new Artist {Name = "CSN&Y"},
                new Artist {Name = "Cream"},

                new Artist {Name = "Don Henley"},
                new Artist {Name = "David Bowie"},
                new Artist {Name = "Derek And The Dominos"},
                new Artist {Name = "Doors"},
                new Artist {Name = "Dire Straits"},
                new Artist {Name = "Don McLean"},
                new Artist {Name = "Doobie Brothers"},
                new Artist {Name = "Deep Purple"},

                new Artist {Name = "Eagles"},
                new Artist {Name = "Elton John"},
                new Artist {Name = "ELP"},
                new Artist {Name = "ELO"},
                new Artist {Name = "Eric Clapton"},
                new Artist {Name = "Edgar Winter Group"},

                new Artist {Name = "Foreigner"},
                new Artist {Name = "Fleetwood Mac"},
                new Artist {Name = "Free"},

                new Artist {Name = "Golden Earring"},
                new Artist {Name = "George Harrison"},
                new Artist {Name = "George Thorogood"},

                new Artist {Name = "Jethro Tull"},
                new Artist {Name = "Jimi Hendrix"},
                new Artist {Name = "Joe Walsh"},
                new Artist {Name = "Janis Joplin"},
                new Artist {Name = "Jefferson Airplane"},

                new Artist {Name = "Heart"},
                new Artist {Name = "Head East"},

                new Artist {Name = "Judas Priest"},

                new Artist {Name = "Kinks"},
                new Artist {Name = "Kansas"},


                new Artist {Name = "Led Zeppelin"},
                new Artist {Name = "Lynyrd Skynyrd"},

                new Artist {Name = "Marshall Tucker Band"},
                new Artist {Name = "Moody Blues"},

                new Artist {Name = "Pink Floyd"},
                new Artist {Name = "Police"},
                new Artist {Name = "Paul McCartney"},
                new Artist {Name = "Peter Frampton"},
                new Artist {Name = "Pete Townshend"},
                new Artist {Name = "Pretenders"},
                new Artist {Name = "Pat Benatar"},

                new Artist {Name = "Queen"},

                new Artist {Name = "Rush"},
                new Artist {Name = "Rolling Stones"},
                new Artist {Name = "Rick Derringer"},
                new Artist {Name = "Rod Stewart"},
                new Artist {Name = "Robin Trower"},

                new Artist {Name = "Santana"},
                new Artist {Name = "Steppenwolf"},
                new Artist {Name = "Steve Miller"},
                new Artist {Name = "Supertramp"},
                new Artist {Name = "Steely Dan"},
                new Artist {Name = "Stevie Ray Vaughan"},
                new Artist {Name = "Styx"},

                new Artist {Name = "Tom Petty"},
                new Artist {Name = "Ten Years After"},
                new Artist {Name = "The Kingsmen"},
                new Artist {Name = "Ted Nugent"},
                new Artist {Name = "Traffic"},

                new Artist {Name = "U2"},
                new Artist {Name = "UB40"},

                new Artist {Name = "Van Halen"},
                new Artist {Name = "Van Morrison"},

                new Artist {Name = "Who"},

                new Artist {Name = "Yes"},

                new Artist {Name = "ZZ Top"},


                new Artist {Name = "Aaron Copland & London Symphony Orchestra"},
                new Artist {Name = "Aaron Goldberg"},
                new Artist {Name = "Accept"},
                new Artist {Name = "Adrian Leaper & Doreen de Feis"},
                new Artist {Name = "Aisha Duo"},
                new Artist {Name = "Alberto Turco & Nova Schola Gregoriana"},
                new Artist {Name = "Amy Winehouse"},
                new Artist {Name = "Anita Ward"},
                new Artist {Name = "Antônio Carlos Jobim"},
                new Artist {Name = "Apocalyptica"},
                new Artist {Name = "Audioslave"},
                new Artist {Name = "Barry Wordsworth & BBC Concert Orchestra"},
                new Artist {Name = "Berliner Philharmoniker & Hans Rosbaud"},
                new Artist {Name = "Berliner Philharmoniker & Herbert Von Karajan"},
                new Artist {Name = "Billy Cobham"},
                new Artist {Name = "Black Label Society"},
                new Artist {Name = "Boston Symphony Orchestra & Seiji Ozawa"},
                new Artist {Name = "Britten Sinfonia, Ivor Bolton & Lesley Garrett"},
                new Artist {Name = "Bruce Dickinson"},
                new Artist {Name = "Buddy Guy"},
                new Artist {Name = "Caetano Veloso"},
                new Artist {Name = "Cake"},
                new Artist {Name = "Calexico"},
                new Artist {Name = "Cássia Eller"},
                new Artist {Name = "Chic"},
                new Artist {Name = "Chicago Symphony Orchestra & Fritz Reiner"},
                new Artist {Name = "Chico Buarque"},
                new Artist {Name = "Chico Science & Nação Zumbi"},
                new Artist {Name = "Choir Of Westminster Abbey & Simon Preston"},
                new Artist {Name = "Chris Cornell"},
                new Artist {Name = "Christopher O'Riley"},
                new Artist {Name = "Cidade Negra"},
                new Artist {Name = "Cláudio Zoli"},
                new Artist {Name = "David Coverdale"},
                new Artist {Name = "Dennis Chambers"},
                new Artist {Name = "Djavan"},
                new Artist {Name = "Donna Summer"},
                new Artist {Name = "Dread Zeppelin"},
                new Artist {Name = "Ed Motta"},
                new Artist {Name = "Edo de Waart & San Francisco Symphony"},
                new Artist {Name = "Elis Regina"},
                new Artist {Name = "English Concert & Trevor Pinnock"},
                new Artist {Name = "Eugene Ormandy"},
                new Artist {Name = "Faith No More"},
                new Artist {Name = "Falamansa"},
                new Artist {Name = "Frank Zappa & Captain Beefheart"},
                new Artist {Name = "Fretwork"},
                new Artist {Name = "Funk Como Le Gusta"},
                new Artist {Name = "Gerald Moore"},
                new Artist {Name = "Gilberto Gil"},
                new Artist {Name = "Godsmack"},
                new Artist {Name = "Gonzaguinha"},
                new Artist {Name = "Göteborgs Symfoniker & Neeme Järvi"},
                new Artist {Name = "Guns N' Roses"},
                new Artist {Name = "Gustav Mahler"},
                new Artist {Name = "Incognito"},
                new Artist {Name = "Iron Maiden"},
                new Artist {Name = "James Levine"},
                new Artist {Name = "Jamiroquai"},
                new Artist {Name = "Joe Satriani"},
                new Artist {Name = "Jorge Ben"},
                new Artist {Name = "Jota Quest"},
                new Artist {Name = "Julian Bream"},
                new Artist {Name = "Kent Nagano and Orchestre de l'Opéra de Lyon"},
                new Artist {Name = "Legião Urbana"},
                new Artist {Name = "Lenny Kravitz"},
                new Artist {Name = "Les Arts Florissants & William Christie"},
                new Artist {Name = "London Symphony Orchestra & Sir Charles Mackerras"},
                new Artist {Name = "Luciana Souza/Romero Lubambo"},
                new Artist {Name = "Lulu Santos"},
                new Artist {Name = "Marcos Valle"},
                new Artist {Name = "Marillion"},
                new Artist {Name = "Marisa Monte"},
                new Artist {Name = "Martin Roscoe"},
                new Artist {Name = "Maurizio Pollini"},
                new Artist {Name = "Mela Tenenbaum, Pro Musica Prague & Richard Kapp"},
                new Artist {Name = "Men At Work"},
                new Artist {Name = "Metallica"},
                new Artist {Name = "Michael Tilson Thomas & San Francisco Symphony"},
                new Artist {Name = "Miles Davis"},
                new Artist {Name = "Milton Nascimento"},
                new Artist {Name = "Mötley Crüe"},
                new Artist {Name = "Motörhead"},
                new Artist {Name = "Nash Ensemble"},
                new Artist {Name = "Nicolaus Esterhazy Sinfonia"},
                new Artist {Name = "Nirvana"},
                new Artist {Name = "O Terço"},
                new Artist {Name = "Olodum"},
                new Artist {Name = "Orchestra of The Age of Enlightenment"},
                new Artist {Name = "Os Paralamas Do Sucesso"},
                new Artist {Name = "Ozzy Osbourne"},
                new Artist {Name = "Paul D'Ianno"},
                new Artist {Name = "Raul Seixas"},
                new Artist {Name = "Roger Norrington, London Classical Players"},
                new Artist {Name = "Royal Philharmonic Orchestra & Sir Thomas Beecham"},
                new Artist {Name = "Scholars Baroque Ensemble"},
                new Artist {Name = "Scorpions"},
                new Artist {Name = "Sergei Prokofiev & Yuri Temirkanov"},
                new Artist {Name = "Sir Georg Solti & Wiener Philharmoniker"},
                new Artist {Name = "Skank"},
                new Artist {Name = "Soundgarden"},
                new Artist {Name = "Spyro Gyra"},
                new Artist {Name = "Stevie Ray Vaughan & Double Trouble"},
                new Artist {Name = "Stone Temple Pilots"},
                new Artist {Name = "System Of A Down"},
                new Artist {Name = "Temple of the Dog"},
                new Artist {Name = "Terry Bozzio, Tony Levin & Steve Stevens"},
                new Artist {Name = "The 12 Cellists of The Berlin Philharmonic"},
                new Artist {Name = "The Black Crowes"},
                new Artist {Name = "The King's Singers"},
                new Artist {Name = "The Posies"},
                new Artist {Name = "Tim Maia"},
                new Artist {Name = "Ton Koopman"},
                new Artist {Name = "Various Artists"},
                new Artist {Name = "Velvet Revolver"},
                new Artist {Name = "Vinícius De Moraes"},
                new Artist {Name = "Wilhelm Kempff"},
                new Artist {Name = "Yehudi Menuhin"},
                new Artist {Name = "Yo-Yo Ma"},
                new Artist {Name = "Zeca Pagodinho"},

                // Country Artists
                new Artist {Name = "Hank Williams"},
                new Artist {Name = "George Jones"},
                new Artist {Name = "Bill Monroe"},
                new Artist {Name = "The Carter Family"},
                new Artist {Name = "Tammy Wynette"},
                new Artist {Name = "Patsy Cline"},
                new Artist {Name = "Randy Travis"},
                new Artist {Name = "Eddy Arnold"},
                new Artist {Name = "Johnny Cash"},
                new Artist {Name = "Dolly Parton"},
                new Artist {Name = "Roy Acuff"},
                new Artist {Name = "Willie Nelson"},
                new Artist {Name = "Garth Brooks"},
                new Artist {Name = "Loretta Lynn"},
                new Artist {Name = "Jimmie Rodgers"},
                new Artist {Name = "Hank Snow"},
                new Artist {Name = "Bob Wills"},
                new Artist {Name = "Kitty Wells"},
                new Artist {Name = "Merle Haggard"},
                new Artist {Name = "Conway Twitty"}
            };

            //try
            //{
            //    new List<Product>
            //    {
            //        // Country

            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Your Cheatin' Heart",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Hank Williams"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "He Stopped Loving Her Today",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "George Jones"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Blue Moon of Kentucky",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bill Monroe"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Can The Circle Be Unbroken",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "The Carter Family"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Stand By Your Man",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Tammy Wynette"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I'm So Lonesome I Could Cry",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Hank Williams"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Crazy",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Patsy Cline"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Forever & Ever, Amen",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Randy Travis"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Make The World Go Away",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eddy Arnold"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I Walk The Line",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Johnny Cash"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I Will Always Love You",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Dolly Parton"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Wabash Cannonball",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Roy Acuff"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Blue Eyes Crying In The Rain",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Willie Nelson"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Dance",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Garth Brooks"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I Fall To Pieces",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Patsy Cline"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Blue Yodel",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Loretta Lynn"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I'm Moving On",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Hank Snow"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Ring of Fire",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Johnny Cash"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "New San Antonio Rose",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bob Wills"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "It Wasn't God",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Kitty Wells"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Mama Tried ",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Merle Haggard"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Friends In Low Places",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Garth Brooks"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Always On My Mind",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Willie Nelson"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Hello Darlin' ",
            //            Genre = genres.Single(g => g.Name == "Country"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Conway Twitty"),
            //            AlbumArtUrl = "placeholder.png"
            //        },

            //        // Rock Albums

            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Stairway to Heaven",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Hey Jude",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "All Along the Watchtower",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jimi Hendrix"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Satisfaction",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rolling Stones"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Like A Rolling Stone",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bob Dylan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Another Brick In The Wall",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pink Floyd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Won't Get Fooled Again",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Who"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Hotel California",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eagles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Layla",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Derek And The Dominos"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Sweet Home Alabama",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Lynyrd Skynyrd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bohemian Rhapsody",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Queen"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Riders on the Storm",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Doors"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Rock and Roll",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Barracuda",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Heart"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "La Grange",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "ZZ Top"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Dream On",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Aerosmith"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "You Really Got Me",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Van Halen"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "More Than a Feeling",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Boston"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Sultans of Swing",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Dire Straits"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "You Shook Me All Night Long",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "AC/DC"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Kashmir",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Lola",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Kinks"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Carry on Wayward Son",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Kansas"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Tiny Dancer",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Elton John"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Locomotive Breath",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jethro Tull"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I Still Haven't Found",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "U2"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Magic Carpet Ride",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Steppenwolf"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Free Bird",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Lynyrd Skynyrd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Purple Haze",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jimi Hendrix"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Tom Sawyer",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rush"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Let It Be",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Baba O'Riley",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Who"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Joker",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Steve Miller"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Roxanne",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Police"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Time",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pink Floyd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "It's A Long Way to the Top",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "AC/DC"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Whole Lotta Love",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Chain",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Fleetwood Mac"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I've Seen All Good People",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Yes"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "For What It's Worth",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Buffalo Springfield"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Black Magic Woman",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Santana"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Nights in White Satin",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Moody Blues"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "While My Guitar Gently Weeps",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Gimme Shelter",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rolling Stones"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Gold Dust Woman",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Fleetwood Mac"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Fortunate Son",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "CCR"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "American Pie",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Don McLean"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bad Company",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bad Company"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Waitin' For The Bus",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "ZZ Top"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Over the Hills and Far Away",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Owner of a Lonely Heart",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Yes"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Logical Song",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Supertramp"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "A Day in the Life",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Sweet Emotion",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Aerosmith"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Down On The Corner",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "CCR"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "My Sweet Lord",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "George Harrison"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Knockin' on Heaven's Door",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bob Dylan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Just What I Needed",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cars"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Don't Fear the Reaper",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Blue Oyster Cult"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Behind Blue Eyes",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Who"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Do It Again",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Steely Dan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Who Do You Love",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "George Thorogood"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "From the Beginning",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "ELP"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Already Gone",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eagles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Here Comes The Sun",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "With Or Without You",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "U2"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Life's Been Good",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Joe Walsh"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Breakdown",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Tom Petty"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Comfortably Numb",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pink Floyd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Ramble On",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I'd Love to Change the World",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ten Years After"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Longtime",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Boston"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Brown Eyed Girl",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Van Morrison"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Back In Black",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "AC/DC"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "You Can't Always Get What You",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rolling Stones"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Take It Easy",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eagles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Sgt. Pepper/With A Little Help",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "We Will Rock You",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Queen"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Dancing Days",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Turn the Page",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bob Seger"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "All Right Now",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Free"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Black Water",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Doobie Brothers"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Oh Well",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Fleetwood Mac"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Me and Bobby McGee",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Janis Joplin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Rocket Man",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Elton John"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Ohio",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "CSN&Y"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "You Really Got Me",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Kinks"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bloody Well Right",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Supertramp"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Dirty Deeds",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "AC/DC"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Aqualung",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jethro Tull"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Wind Cries Mary",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jimi Hendrix"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Burnin' for You",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Blue Oyster Cult"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Moving in Stereo",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cars"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "House of the Rising Sun",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Animals"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bargain",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Who"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Maybe I'm Amazed",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Paul McCartney"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bennie & The Jets",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Elton John"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Dust in the Wind",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Kansas"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Rock 'n Roll Hootchie Koo",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rick Derringer"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Crazy On You",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Heart"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Do You Feel Like We Do",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Peter Frampton"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Louie Louie",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "The Kingsmen"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Jessica",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Allman Brothers"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Long Train Running",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Doobie Brothers"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Walkin' On The Moon",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Police"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Stranglehold",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ted Nugent"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Fire",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jimi Hendrix"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Tush",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "ZZ Top"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Feel Like Making Love",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bad Company"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Who'll Stop The Rain",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "CCR"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Sky Is Crying",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Stevie Ray Vaughan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "You Ain't Seen Nothing Yet",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "BTO"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Smoke on the Water",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Deep Purple"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Can't You See",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Marshall Tucker Band"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Night Moves",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bob Seger"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Touch Me",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Doors"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Runnin' With The Devil",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Van Halen"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Cocaine",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eric Clapton"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Immigrant Song",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Run Through The Jungle",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "CCR"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Brain Damage",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pink Floyd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "You've Got to Hide Your Love Away",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Jumpin' Jack Flash",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rolling Stones"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Levon",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Elton John"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Take The Money And Run",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Steve Miller"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Maggie May",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rod Stewart"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Born To Be Wild",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Steppenwolf"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "White Room",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cream"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Lucky Man",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "ELP"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Call Me The Breeze",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Lynyrd Skynyrd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "My Best Friend's Girl",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cars"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Let My Love Open The Door",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pete Townshend"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Money For Nothing",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Dire Straits"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Breakfast In America",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Supertramp"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "You Make Lovin' Fun",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Fleetwood Mac"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Burnin' Sky",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bad Company"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Long Distance Runaround",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Yes"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Grand Illusion",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Styx"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Strawberry Fields Forever",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Come Together",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Goodbye Yellow Brick Road",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Elton John"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Rock 'n Roll Band",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Boston"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Runnin' Down a Dream",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Tom Petty"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I Heard It Through The Grapevine",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "CCR"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Dear Mr. Fantasy",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Traffic"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Dreams",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Fleetwood Mac"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Fire on High",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "ELO"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Wish You Were Here",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pink Floyd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bridge of Sighs",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Robin Trower"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Rocky Racoon",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Walk This Way",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Aerosmith"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "In My Life",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Good Times Roll",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cars"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Fool In the Rain",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Revolution",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Born To Run",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bruce Springsteen"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Oye Como Va",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Santana"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Reeling In The Years",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Steely Dan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Every Breath You Take",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Police"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Rainy Day Women",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bob Dylan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Frankenstein",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Edgar Winter Group"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Refugee",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Tom Petty"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Have A Cigar",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pink Floyd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Spirit of Radio",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rush"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Going To California",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Led Zeppelin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Hello, I Love You",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Doors"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Go Your Own Way",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Fleetwood Mac"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Peace of Mind",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Boston"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "White Rabbit",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jefferson Airplane"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Radar Love",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Golden Earring"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Never Been Any Reason",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Head East"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Let It Rain",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eric Clapton"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Cold As Ice",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Foreigner"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Karn Evil",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "ELP"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Subdivisions",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rush"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Pink Cadillac",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bruce Springsteen"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Rocky Mountain Way",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Joe Walsh"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "New Year's Day",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "U2"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Magic Man",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Heart"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Witchy Woman",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eagles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Rock and Roll Fantasy",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bad Company"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Love Me Two Times",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Doors"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Eleanor Rigby",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Middle of The Road",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pretenders"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Sunshine of Your Love",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cream"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Don't Look Back",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Boston"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Dirty Laundry",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Don Henley"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Penny Lane",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Killer Queen",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Queen"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Fly Like An Eagle",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Steve Miller"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Cross-Eyed Mary",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jethro Tull"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Heartbreaker",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pat Benatar"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Norwegian Wood",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Beatles"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Your Song",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Elton John"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Hey Baby",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ted Nugent"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Money",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Pink Floyd"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Space Oddity",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "David Bowie"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Street Fighting Man",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Rolling Stones"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Moondance",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Van Morrison"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Pride and Joy",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Stevie Ray Vaughan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The World Feels Dusty",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Aaron Copland & London Symphony Orchestra"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Salvador",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Aaron Goldberg"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Symphony No. 3: III. Lento - Cantablile Semplice ",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Adrian Leaper & Doreen de Feis"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Beneath an Evening Sky ",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Aisha Duo"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Frank",
            //            Genre = genres.Single(g => g.Name == "Pop"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Amy Winehouse"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Ring My Bell",
            //            Genre = genres.Single(g => g.Name == "Disco"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Anita Ward"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Chill Brazil",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Antônio Carlos Jobim"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Warner 25 Anos",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Antônio Carlos Jobim"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Master of Puppets",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Apocalyptica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Revelations",
            //            Genre = genres.Single(g => g.Name == "Alternative"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Audioslave"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Until We Fall",
            //            Genre = genres.Single(g => g.Name == "Rock"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Audioslave"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Last Night of the Proms",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Barry Wordsworth & BBC Concert Orchestra"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Symphony No. 1 in E Minor",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Berliner Philharmoniker & Hans Rosbaud"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Mozart: Symphonies Nos. 40 & 41",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Berliner Philharmoniker & Herbert Von Karajan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Quadrant 4",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Billy Cobham"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Alcohol Fueled Brewtality Live!",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Black Label Society"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Superterrorizer",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Black Label Society"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Wheels of Confusion ",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Black Sabbath"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Every Day Comes and Goes ",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Black Sabbath"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Carmina Burana",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Boston Symphony Orchestra & Seiji Ozawa"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "A Soprano Inspired",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Britten Sinfonia, Ivor Bolton & Lesley Garrett"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Chemical Wedding",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Bruce Dickinson"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Prenda Minha",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Caetano Veloso"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Sozinho Remix Ao Vivo",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Caetano Veloso"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "War Pigs",
            //            Genre = genres.Single(g => g.Name == "Alternative"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cake"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Carried to Dust",
            //            Genre = genres.Single(g => g.Name == "Alternative"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Calexico"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Cássia Eller - Sem Limite",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cássia Eller"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Le Freak",
            //            Genre = genres.Single(g => g.Name == "Disco"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Chic"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Scheherazade",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Chicago Symphony Orchestra & Fritz Reiner"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Minha Historia",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Chico Buarque"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Afrociberdelia",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Chico Science & Nação Zumbi"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Da Lama Ao Caos",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Chico Science & Nação Zumbi"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Carry On",
            //            Genre = genres.Single(g => g.Name == "Alternative"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Chris Cornell"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "SCRIABIN: Vers la flamme",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Christopher O'Riley"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Acústico MTV",
            //            Genre = genres.Single(g => g.Name == "Reggae"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cidade Negra"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Monte Castelo",
            //            Genre = genres.Single(g => g.Name == "Reggae"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cidade Negra"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Na Pista",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Cláudio Zoli"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Outbreak",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Dennis Chambers"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Um Amor Puro",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Djavan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Oceano",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Djavan"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "MacArthur Park",
            //            Genre = genres.Single(g => g.Name == "Disco"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Donna Summer"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Cassiano",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ed Motta"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Adams, John: The Chairman Dances",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Edo de Waart & San Francisco Symphony"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Elis Regina-Minha História",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Elis Regina"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Pachelbel: Canon & Gigue",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "English Concert & Trevor Pinnock"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bell Bottom Blues",
            //            Genre = genres.Single(g => g.Name == "Blues"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eric Clapton"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Promises",
            //            Genre = genres.Single(g => g.Name == "Blues"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eric Clapton"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Respighi:Pines of Rome",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eugene Ormandy"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Strauss: Waltzes",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Eugene Ormandy"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Deixa Entrar",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Falamansa"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Roda De Funk",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Funk Como Le Gusta"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Quanta Gente Veio Ver (Live)",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Gilberto Gil"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Quanta Gente Veio ver--Bônus De Carnaval",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Gilberto Gil"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Faceless",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Godsmack"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Meus Momentos",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Gonzaguinha"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Nielsen Symphony",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Göteborgs Symfoniker & Neeme Järvi"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Civil War",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Guns N' Roses"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Blue Mood",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Incognito"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "A Real Dead One",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "A Real Live One",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Run to the Hills",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Ghost of the Navigator",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Piece Of Mind",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Fear of the Dark",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Wicker Man",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Number of the Beast",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Seventh Son",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Somewhere in Time",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Dream of Mirrors",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Number of the Beast",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Iron Maiden"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Mascagni: Cavalleria Rusticana",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "James Levine"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Jorge Ben Jor 25 Anos",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jorge Ben"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Jota Quest",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Jota Quest"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Living After Midnight",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Judas Priest"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Weill: The Seven Deadly Sins",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Kent Nagano and Orchestre de l'Opéra de Lyon"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Mais Do Mesmo",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Legião Urbana"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Tchaikovsky: The Nutcracker",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "London Symphony Orchestra & Sir Charles Mackerras"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Duos II",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Luciana Souza/Romero Lubambo"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Chill: Mexico",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Marcos Valle"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Barulhinho Bom",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Marisa Monte"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Szymanowski: Piano Works",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Martin Roscoe"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Shortest Straw",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "To Live Is to Die",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Garage Inc.",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Kill 'Em All",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Unforgiven",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Master Of Puppets",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "ReLoad",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Ride The Lightning",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "St. Anger",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Metallica"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Berlioz: Symphonie Fantastique",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Michael Tilson Thomas & San Francisco Symphony"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Prokofiev: Romeo & Juliet",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Michael Tilson Thomas & San Francisco Symphony"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Miles Ahead",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Miles Davis"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Duke",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Miles Davis"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "New Rhumba",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Miles Davis"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Milton Nascimento Ao Vivo",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Milton Nascimento"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Minas",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Milton Nascimento"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bitter Pill",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Mötley Crüe"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Ace Of Spades",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Motörhead"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Chamber",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Nash Ensemble"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Fifth",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Nicolaus Esterhazy Sinfonia"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Olodum",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Olodum"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bach: The Brandenburg Concertos",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Orchestra of The Age of Enlightenment"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Acústico MTV",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Os Paralamas Do Sucesso"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Arquivo II",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Os Paralamas Do Sucesso"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Arquivo Os Paralamas Do Sucesso",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Os Paralamas Do Sucesso"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Paranoid",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ozzy Osbourne"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Suicide Solution)",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ozzy Osbourne"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Iron Man",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ozzy Osbourne"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Believer",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ozzy Osbourne"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "I Don't Know)",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ozzy Osbourne"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Goodbye to Romance",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ozzy Osbourne"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Purcell: The Fairy Queen",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Roger Norrington, London Classical Players"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Haydn: Symphonies 99",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Royal Philharmonic Orchestra & Sir Thomas Beecham"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Handel: The Messiah",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Scholars Baroque Ensemble"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Prokofiev: Symphony No.1",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Sergei Prokofiev & Yuri Temirkanov"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Wagner: Overture",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Sir Georg Solti & Wiener Philharmoniker"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Heart of the Night",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Spyro Gyra"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Morning Dance",
            //            Genre = genres.Single(g => g.Name == "Jazz"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Spyro Gyra"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "In Step",
            //            Genre = genres.Single(g => g.Name == "Blues"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Stevie Ray Vaughan & Double Trouble"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Mezmerize",
            //            Genre = genres.Single(g => g.Name == "Metal"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "System Of A Down"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Temple of the Dog",
            //            Genre = genres.Single(g => g.Name == "Alternative"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Temple of the Dog"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "South American Getaway",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "The 12 Cellists of The Berlin Philharmonic"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Hard To Handle",
            //            Genre = genres.Single(g => g.Name == "Blues"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "The Black Crowes"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Thick N'Thin",
            //            Genre = genres.Single(g => g.Name == "Blues"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "The Black Crowes"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "English Renaissance",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "The King's Singers"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Serie Sem Limite (Disc 1)",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Tim Maia"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Hapsburg Serenade",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Tim Maia"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bach: Toccata & Fugue in D Minor",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Ton Koopman"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = SetCaution(),
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "The Earth Dies Screaming",
            //            Genre = genres.Single(g => g.Name == "Reggae"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "UB40"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Axé Bahia 2001",
            //            Genre = genres.Single(g => g.Name == "Pop"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Various Artists"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Sambas De Enredo 2001",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Various Artists"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Vozes do MPB",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Various Artists"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Vinicius De Moraes",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Vinícius De Moraes"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bach: Goldberg Variations",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Wilhelm Kempff"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bartok: Violin & Viola Concertos",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Yehudi Menuhin"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Bach: The Cello Suites",
            //            Genre = genres.Single(g => g.Name == "Classical"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Yo-Yo Ma"),
            //            AlbumArtUrl = "placeholder.png"
            //        },
            //        new Product
            //        {
            //            Cutout = false,
            //            ProductId = Guid.NewGuid(),
            //            ParentalCaution = false,
            //            ReleaseDate = SetReleaseDate(),
            //            Upc = GenerateUpc(),
            //            Title = "Ao Vivo",
            //            Genre = genres.Single(g => g.Name == "Latin"),
            //            Price = GenerateAlbumPrice(),
            //            Artist = artists.Single(a => a.Name == "Zeca Pagodinho"),
            //            AlbumArtUrl = "placeholder.png"
            //        }
            //    }.ForEach(a => context.Products.Add(a));

            //    try
            //    {
            //        context.SaveChanges();
            //    }
            //    catch (Exception ex)
            //    {
            //        // Todo: Add Logging
            //        var errorMessage = $"Error seeding Product Catalog data {ex.Message}";
            //        throw;
            //    }
            //}
            //catch (InvalidOperationException ex)
            //{
            //    var errorMessage = $"Error seeding Product Catalog data {ex.Data}";
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    var errorMessage = $"Error seeding Product Catalog data {ex.Message}";
            //    throw;
            //}
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
    }
}