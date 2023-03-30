using System;

namespace Catalog.API.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AlbumArtUrl { get; set; }

        public bool ParentalCaution { get; set; }

        public string Upc { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal Price { get; set; }

        public int Available { get; set; }

        public bool? Cutout { get; set; }

        public string ArtistName { get; set; }

        public int ArtistId { get; set; }

        public string GenreName { get; set; }

        public int GenreId { get; set; }
    }
}