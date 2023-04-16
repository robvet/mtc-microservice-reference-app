using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Domain.Entities;

namespace Catalog.API.Contracts
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Task<List<Artist>> GetAll(string correlationToken);
        Task<Artist> GetById(int id, string correlationToken);
    }
}