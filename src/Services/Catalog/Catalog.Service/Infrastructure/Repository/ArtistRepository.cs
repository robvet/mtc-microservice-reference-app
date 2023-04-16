using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Contracts;
using Catalog.API.Domain.Entities;
using Catalog.API.Infrastructure.DataStore;

namespace Catalog.API.Infrastructure.Repository
{
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(DataContext ctx) : base(ctx)
        {
        }

        public async Task<Artist> GetById(int id, string correlationToken)
        {
            return await FindById(id);
        }

        public async Task<List<Artist>> GetAll(string correlationToken)
        {
            return Get().ToList();
        }
    }
}