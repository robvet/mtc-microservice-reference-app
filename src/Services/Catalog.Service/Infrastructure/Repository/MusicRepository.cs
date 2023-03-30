using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Contracts;
using Catalog.API.Domain.Entities;
using Catalog.API.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure.Repository
{
    public class MusicRepository : BaseRepository<Product>, IMusicRepository
    {
        public MusicRepository(DataContext ctx) : base(ctx)
        {
        }

        public async Task<int> GetCount(string correlationToken)
        {
            return await Get().CountAsync();
        }

        public async Task<List<Product>> GetTopSellers(int count, string correlationToken)
        {
            // Group the order details by album and return the albums with the highest count

            // Fix for race conditions where music is created with higher id values
            // Select MIN(id) from products as lowestId
            var lowestId = Get().Min(x => x.Id);

            var topSellers = new List<Product>();
            var rnd = new Random();
            var productCount = Get().Count() + 5;

            for (var i = 0; i <= count; i++)
            {
                var item = rnd.Next(productCount);

                var selecteditem = Get()
                    .Where(x => x.Id == (item + lowestId))
                    .AsNoTracking() // Disable change tracking
                    .FirstOrDefault();

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
            return Get().Include(x => x.Artist).Include(y => y.Genre).ToList();
        }

        public async Task<Product> GetById(int id, string correlationToken)
        {
            return Get().Where(x => x.Id == id).Include(x => x.Artist).Include(y => y.Genre).FirstOrDefault();
        }

        public async Task<Product> GetByIdWithIdempotencyCheck(int id, string correlationToken)
        {
            var guid = ParseCorrelationToken(correlationToken);
            
            return Get().Where(x => x.Id == id && x.CorrelationId == guid).Include(x => x.Artist)
                .Include(y => y.Genre).FirstOrDefault();
        }


        public async Task<bool> ChangeParentalCaution(int albumId, bool parentalCaution, string correlationToken)
        {
            var album = await GetById(albumId, correlationToken);

            if (album == null)
                return false;

            // If ParentalCaution has not changed, the short-circuit operation
            // and return true, avoiding expense of unneccesary update.
            if (album.ParentalCaution == parentalCaution)
                return true;

            album.ParentalCaution = parentalCaution;

            await Update(album);

            return true;
        }

        public async Task<List<Product>> RetrieveArtistsForGenre(int genreId, string correlationToken)
        {
            return Get()
                .Include("Artists")
                .Where(x => x.GenreId == genreId).ToList();
        }

        public async Task<List<Product>> GetInexpensiveAlbumsByGenre(int genreId, decimal priceCeiling,
            string correlationToken)
        {
            throw new NotImplementedException();
            //return base.Find(x => x.GenreId == genreId && x.Price <= priceCeiling).ToList();
        }

        /// <summary>
        /// Parse out extra characters to make guid
        /// </summary>
        /// <param name="correlationToken"></param>
        /// <returns></returns>
        private Guid ParseCorrelationToken(string correlationToken)
        {
            var count = correlationToken.IndexOf("-") + 1;
            return new Guid(correlationToken.Substring(count));
        }
    }
}