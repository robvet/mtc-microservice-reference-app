using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tools.Entities;

namespace Tools.Database
{
    public class MusicRepository : BaseRepository<Product>, IMusicRepository
    {
        public MusicRepository(DataContext ctx) : base(ctx)
        {
        }

        public int GetCount(string correlationToken)
        {
            return Get().Count();
        }


        public IList<Product> GetAll(string correlationToken)
        {
            return Get().Include(x => x.Artist).Include(y => y.Genre).ToList();
        }

        public Product GetById(int id, string correlationToken)
        {
            return Find(x => x.Id == id).Include(x => x.Artist).Include(y => y.Genre).FirstOrDefault();
        }

        public bool ChangeParentalCaution(int albumId, bool parentalCaution, string correlationToken)
        {
            var album = GetById(albumId, correlationToken);

            if (album == null)
                return false;

            // If ParentalCaution has not changed, the short-circuit operation
            // and return true, avoiding expense of unneccesary update.
            if (album.ParentalCaution == parentalCaution)
                return true;

            album.ParentalCaution = parentalCaution;

            Update(album);

            return true;
        }

        public IList<Product> RetrieveArtistsForGenre(int genreId, string correlationToken)
        {
            return Get()
                .Include("Artists")
                .Where(x => x.GenreId == genreId).ToList();
        }

        public IList<Product> GetInexpensiveAlbumsByGenre(int genreId, decimal priceCeiling, string correlationToken)
        {
            throw new NotImplementedException();
            //return base.Find(x => x.GenreId == genreId && x.Price <= priceCeiling).ToList();
        }
    }
}