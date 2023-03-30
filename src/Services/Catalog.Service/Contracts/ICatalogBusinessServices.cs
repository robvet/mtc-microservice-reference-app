using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Domain.Entities;

namespace Catalog.API.Contracts
{
    public interface ICatalogBusinessServices
    {
        Task<List<Product>> GetAllMusic(string correlationToken);
        Task<Product> GetMusic(string correlationToken, int albumId);
        Task<List<Product>> GetTopSellingMusic(string correlationToken, int count);
        Task<List<Genre>> GetAllGenres(string correlationToken, bool includeAlbums = false);
        Task<List<Artist>> GetAllArtists(string correlationToken);
        Task<Artist> GetArtist(int artistID, string correlationToken);
        Task Add(string correlationToken, Product product);
        Task Update(string correlationToken, Product product);
        Task<Genre> GetGenre(int genreId, string correlationToken, bool includeAlbums = false);
    }
}