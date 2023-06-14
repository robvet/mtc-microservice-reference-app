using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Domain.Entities;

namespace catalog.service.Contracts
{
    public interface ICatalogBusinessServices
    {
        Task<List<Product>> GetAllMusic(string correlationToken);
        Task<Product> GetMusic(string correlationToken, int albumId);
        Task<List<Product>> GetTopSellingMusic(string correlationToken, int count);
        Task<List<Genre>> GetAllGenres(string correlationToken);
        Task<List<Artist>> GetAllArtists(string correlationToken);
        Task<Artist> GetArtist(int artistID, string correlationToken);
        
        //Task Add(string correlationToken, Product product);
        //Task Update(string correlationToken, Product product);

        Task<Genre> GetGenre(int genreId, string correlationToken);
        Task<List<Product>> GetMusicForGenre(int genreId, string correlationToken);
        Task<List<Product>> GetMusicForArtist(int artistId, string correlationToken);
    }
}