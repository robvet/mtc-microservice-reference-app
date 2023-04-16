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
                ? Get().Include(x => x.Albums).SingleOrDefault(g => g.GenreId == id)
                : await FindById(id);
        }

        public async Task<List<Genre>> GetAll(string correlationToken)
        {
            return Get().ToList();
        }

        public async Task<List<Genre>> GetAllAndAlbums(string correlationToken)
        {
            return Get().Include(x => x.Albums).ToList();
        }

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
            return Get().Include(x => x.Albums).SingleOrDefault(x => x.Name == genre);
        }
    }
}