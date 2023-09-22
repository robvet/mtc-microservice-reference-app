using catalog.service.Domain.Entities;
using System.Collections.Generic;

namespace catalog.service.Domain.Entities
{
    public class ProductLookupData
    {
        public ICollection<Artist> Artists { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Medium> Mediums { get; set; }

        public ProductLookupData()
        {
            
        }

        //public ProductLookupData()
        //{
        //    Artists = new List<Artist>();
        //    Genres = new List<Genre>();
        //    Mediums = new List<Medium>();
        //}

        public bool IsArtistsCollectionEmpty()
        {
            return Artists.Count == 0;
        }

        public bool IsGenresCollectionEmpty()
        {
            return Genres.Count == 0;
        }

        public bool IsMediumsCollectionEmpty()
        {
            return Mediums.Count == 0;
        }

        public void AssignArtistToCollection(Artist artist)
        {
            if (artist != null)
                Artists.Add(artist);
        }

        public void AssignGenreToCollection(Genre genre)
        {
            if (genre != null)
                Genres.Add(genre);
        }

        public void AssignMediumToCollection(Medium medium)
        {
            if (medium != null)
                Mediums.Add(medium);
        }
        public IEnumerable<Artist> GetArtists()
        {
            return Artists;
        }

        public ICollection<Genre> DuplicateGenres()
        {
            return new List<Genre>(Genres);
        }

        public ICollection<Medium> DuplicateMediums()
        {
            return new List<Medium>(Mediums);
        }
       
    }
}

