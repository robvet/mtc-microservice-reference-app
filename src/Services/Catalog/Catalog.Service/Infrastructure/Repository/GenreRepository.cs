using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Contracts;
using Catalog.API.Domain.Entities;
using Catalog.API.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure.Repository
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(DataContext ctx) : base(ctx)
        {
        }

        public async Task<Genre> GetById(int id, string correlationToken, bool includeAlbums = false)
        {
            return includeAlbums
                ? Get().Include(x => x.Products).SingleOrDefault(g => g.GenreId == id)
                : await FindById(id);
        }

        public async Task<List<Genre>> GetAll(string correlationToken, bool includeProducts)
        {
            // Important to return empty product list if no products exist
            // Avoids errors in UX
            if (IsEmpty())
            {
                return new List<Genre>();
            }

            if (includeProducts)
            {
                return await Get().Include(x => x.Products).ToListAsync();
            }

            return await Get().ToListAsync();
        }

        //public async Task<List<Genre>> GetAllAndAlbums(string correlationToken)
        //{
            
        //    return Get().Include(x => x.Products).ToList();
        //}

        //public override void Add(Genre genre)
        //{
        //    Update(genre);
        //    // Explicitly call save changes as we will need 
        //    // the new Id to create the Location Header.
        //    // With UnitOfWork, it does not call Save
        //    // until after the controller action executes.
        //    Save();
        //}


        public async Task<Genre> GetGenreAndAlbums(string genre, string correlationToken)
        {
            return await Get().Include(x => x.Products).FirstOrDefaultAsync(x => x.Name == genre);
            //return       Get().Include(x => x.Products).SingleOrDefault(x => x.Name == genre);
        }
    }
}