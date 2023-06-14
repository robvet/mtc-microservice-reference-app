using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catalog.service.Contracts;
using catalog.service.Domain.Entities;
using catalog.service.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;

namespace catalog.service.Infrastructure.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DataContext ctx) : base(ctx)
        {
        }

        public async Task<int> GetCount(string correlationToken)
        {
            return await Get().CountAsync();
        }

        public async Task<List<Product>> GetTopSellers(int count, string correlationToken)
        {
            // Group the order details by album and return the albums with the highest count

            // Important to return empty product list if no products exist
            // Avoids errors in UX
            if (IsEmpty())
            {
                return new List<Product>();
            }

            // Fix for race conditions where music is created with higher id values
            // Select MIN(id) from products as lowestId
            var lowestId = await Task.Run(() => Get().Min(x => x.Id));
            //var lowestId = Get().Min(x => x.Id);

            var topSellers = new List<Product>();
            var rnd = new Random();
            var productCount = Get().Count() + 5;

            for (var i = 1; i <= count; i++)
            {
                var item = rnd.Next(productCount);

                var selecteditem = await Get()
                        .Include(x => x.Artist)
                        .Include(y => y.Genre)
                        .Include(z => z.Medium)
                        .Include(a => a.Status)
                        .Include(b => b.Condition)
                        .Where(x => x.Id == item + lowestId)
                        .AsNoTracking() // Disable change tracking
                        .FirstOrDefaultAsync();


                //var selecteditem = await Get()
                //    .Where(x => x.Id == (item + lowestId))
                //    .AsNoTracking() // Disable change tracking
                //    .FirstOrDefault();


                // In case selected item does not exist
                if (selecteditem == null)
                    i--;
                else
                    topSellers.Add(selecteditem);
            }

            return topSellers;

            //return Get()
            //    // TODO: Add logic that reads orders to build Top Sellers list 
            //    //       based upon orders
            //    //.OrderByDescending(x => x.OrderDetails.Count())
            //    .Take(count)
            //    .AsNoTracking() // Disable change tracking
            //    .Include(x => x.Artist)
            //    .Include(y => y.Genre)
            //    .ToList();


            //Random rnd = new Random();
            //List<blogobj> blogList = CFD.GetMyBlogList();
            //var _randomizedList = from item in blogList
            //    orderby rnd.Next()
            //    select item;
        }

        public async Task<List<Product>> GetAll(string correlationToken)
        {
            // Important to return empty product list if no products exist
            // Avoids errors in UX
            if (IsEmpty())
            {
                return new List<Product>();
            }

            return await Get().Include(x => x.Artist)
                        .Include(y => y.Genre)
                        .Include(z => z.Medium)
                        .Include(a => a.Status)
                        .Include(b => b.Condition)
                        .ToListAsync();
        }

        public async Task<Product> GetById(int id, string correlationToken)
        {
            return await Get().Where(x => x.Id == id)
                        .Include(x => x.Artist)
                        .Include(y => y.Genre)
                        .Include(z => z.Medium)
                        .Include(a => a.Status)
                        .Include(b => b.Condition)
                        .FirstOrDefaultAsync();
        }

        public async Task<Product> GetByIdWithIdempotencyCheck(int id, Guid productId, string correlationToken)
        {
            return await Get().Where(x => x.ProductId == productId)
                        .Include(x => x.Artist)
                        .Include(y => y.Genre)
                        .Include(z => z.Medium)
                        .Include(a => a.Status)
                        .Include(b => b.Condition)
                        .FirstOrDefaultAsync();
            //return Get().Where(x => x.Id == id && x.ProductId == guid).Include(x => x.Artist)
            //    .Include(y => y.Genre).FirstOrDefault();
        }


        public async Task<bool> ChangeParentalCaution(int albumId, bool parentalCaution, string correlationToken)
        {
            var album = await GetById(albumId, correlationToken);

            if (album == null)
                return false;

            // If ParentalCaution hasn't changed, short-circuit operation
            // and return true, avoiding expense of unneccesary database update operation.
            if (album.ParentalCaution == parentalCaution)
                return true;

            album.ParentalCaution = parentalCaution;

            await Update(album);

            return true;
        }

        public async Task<List<Product>> GetProductsForGenre(int genreId, string correlationToken)
        {
            return await Get().Include(x => x.Artist)
                        .Include(y => y.Genre)
                        .Include(z => z.Medium)
                        .Include(a => a.Status)
                        .Include(b => b.Condition)
                        .Where(x => x.GenreId == genreId)
                        .ToListAsync();
        }

        public async Task<List<Product>> GetProductsForArtist(int artistId, string correlationToken)
        {
            return await Get()
                        .Where(x => x.ArtistId == artistId)
                        .Include(x => x.Artist)
                        .Include(y => y.Genre)
                        .Include(z => z.Medium)
                        .Include(a => a.Status)
                        .Include(b => b.Condition)
                        .ToListAsync();
        }


        public async Task<List<Product>> RetrieveArtistsForGenre(int genreId, string correlationToken)
        {
            return await Get()
                .Include("Artists")
                .Where(x => x.GenreId == genreId).ToListAsync();
        }
    }
}