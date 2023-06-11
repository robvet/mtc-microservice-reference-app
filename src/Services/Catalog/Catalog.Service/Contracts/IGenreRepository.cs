﻿using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Domain.Entities;

namespace catalog.service.Contracts
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<Genre> GetById(int id, string correlationToken, bool includeAlbums = false);
        Task<List<Genre>> GetAll(string correlationToken, bool includeProducts);
        Task<List<Genre>> GetAllAndAlbums(string correlationToken);
    }
}