using System;
using Newtonsoft.Json;

namespace Catalog.API.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; }
        public string AlbumArtUrl { get; set; }
        public decimal Price { get; set; }
        public bool ParentalCaution { get; set; }
        public bool? Cutout { get; set; }
        public string Upc { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public Guid CorrelationId { get; set; }

        public Artist Artist { get; set; }
        public Genre Genre { get; set; }
    }
}