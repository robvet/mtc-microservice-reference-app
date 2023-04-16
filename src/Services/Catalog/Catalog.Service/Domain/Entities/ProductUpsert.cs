using System;

namespace Catalog.API.Domain.Entities
{
    public class ProductUpsert
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AlbumArtUrl { get; set; }

        public bool ParentalCaution { get; set; }

        public string Upc { get; set; }

        public bool? Cutout { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal Price { get; set; }

        public int ArtistId { get; set; }

        public int GenreId { get; set; }
    }
}