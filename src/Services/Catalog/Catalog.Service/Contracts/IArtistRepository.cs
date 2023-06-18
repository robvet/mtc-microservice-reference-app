using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Domain.Entities;

namespace catalog.service.Contracts
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Task<List<Artist>> GetAll(string correlationToken);
        Task<Artist> GetById(Guid guidId, string correlationToken);
    }
}