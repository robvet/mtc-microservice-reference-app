using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace catalog.service.Contracts
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
    }
}