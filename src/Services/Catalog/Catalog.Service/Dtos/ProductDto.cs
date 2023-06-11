using System;

namespace catalog.service.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public Guid ProductId { get; set; }

        public string Title { get; set; }

        public string Single { get; set; }

        public string AlbumArtUrl { get; set; }

        public bool ParentalCaution { get; set; }

        public string Upc { get; set; }

        public string ReleaseYear { get; set; }

        public decimal Price { get; set; }

        public string ArtistName { get; set; }
        public int ArtistId { get; set; }

        public string GenreName { get; set; }
        public int GenreId { get; set; }

        public string MediumName { get; set; }
        public int MediumId { get; set; }

        public string StatusName { get; set; }
        public int StatusId { get; set; }

        public string ConditionName { get; set; }
        public int ConditionId { get; set; }

        public bool IsActive { get; set; }
    }
}