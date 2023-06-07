﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Domain.Entities;

namespace Catalog.API.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<int> GetCount(string correlationToken);
        Task<List<Product>> GetTopSellers(int count, string correlationToken);
        Task<List<Product>> GetAll(string correlationToken);
        Task<Product> GetById(int id, string correlationToken);
        Task<bool> ChangeParentalCaution(int albumId, bool parentalCaution, string correlationToken);
        Task<List<Product>> RetrieveArtistsForGenre(int genreId, string correlationToken);
        Task<Product> GetByIdWithIdempotencyCheck(int id, Guid productId, string correlationToken);
        Task ClearProductDatabase(string correlationToken);
    }
}