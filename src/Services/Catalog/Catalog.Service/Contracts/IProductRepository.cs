using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Domain.Entities;

namespace catalog.service.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<int> GetCount(string correlationToken);
        Task<List<Product>> GetTopSellers(int count, string correlationToken);
        Task<List<Product>> GetAll(string correlationToken);
        Task<Product> GetById(Guid id, string correlationToken);
        Task<bool> ChangeParentalCaution(Guid productId, bool parentalCaution, string correlationToken);
        //Task<List<Product>> RetrieveArtistsForGenre(int genreId, string correlationToken);
        Task<Product> GetByIdWithIdempotencyCheck(Guid productId, string correlationToken);
        Task<List<Product>> GetProductsForGenre(Guid guidId, string correlationToken);
        Task<List<Product>> GetProductsForArtist(Guid guidId, string correlationToken);
    }
}