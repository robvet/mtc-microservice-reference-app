using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Domain.Entities;

namespace catalog.service.Contracts
{
    public interface ICatalogBusinessServices
    {
        Task<List<Product>> GetAllMusic(string correlationToken);
        Task<Product> GetMusic(string correlationToken, Guid albumId);
        Task<List<Product>> GetTopSellingMusic(string correlationToken, int count);
        Task<List<Genre>> GetAllGenres(string correlationToken);
        Task<List<Artist>> GetAllArtists(string correlationToken);
        Task<Artist> GetArtist(Guid guidId, string correlationToken);
        Task<Medium> GetMedium(Guid guidId, string correlationToken);
        Task<List<Medium>> GetAllMediums(string correlationToken);

        //Task Add(string correlationToken, Product product);
        //Task Update(string correlationToken, Product product);

        Task<Genre> GetGenre(Guid guidId, string correlationToken);
        Task<List<Product>> GetMusicForGenre(Guid guidId, string correlationToken);
        Task<List<Product>> GetMusicForArtist(Guid guidId, string correlationToken);
        Task<List<Product>> GetMusicForMedium(Guid guidId, string correlationToken);
    }
}