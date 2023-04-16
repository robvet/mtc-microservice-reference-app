using System;
using System.Runtime.Serialization;

namespace Basket.API.Domain.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AlbumArtUrl { get; set; }
        public bool ParentalCaution { get; set; }
        public string Upc { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public bool? Cutout { get; set; }
        public string ArtistName { get; set; }
        public string GenreName { get; set; }
    }
}