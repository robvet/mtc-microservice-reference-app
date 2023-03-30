using System.Collections.Generic;
using Tools.Entities;

namespace Tools.Database
{
    public interface IMusicRepository : IRepository<Product>
    {
        int GetCount(string correlationToken);
        IList<Product> GetAll(string correlationToken);
        Product GetById(int id, string correlationToken);
        bool ChangeParentalCaution(int albumId, bool parentalCaution, string correlationToken);
        IList<Product> RetrieveArtistsForGenre(int genreId, string correlationToken);
        IList<Product> GetInexpensiveAlbumsByGenre(int genreId, decimal priceCeiling, string correlationToken);
    }
}