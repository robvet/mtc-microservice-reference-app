using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace catalog.service.Domain.Entities
{
    public class Product
    {
        public Product()
        {
            Descriptions = new HashSet<Description>();
        }

        public int Id { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public int StatusId { get; set; }
        public int MediumId { get; set; }
        public int ConditionId { get; set; }

        public string Title { get; set; }
        public string Single { get; set; }
        public string AlbumArtUrl { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public bool ParentalCaution { get; set; }
        public string Upc { get; set; }
        public string ReleaseYear { get; set; }
        public Guid ProductId { get; set; }

        public Artist Artist { get; set; }
        public Genre Genre { get; set; }
        public Status Status { get; set; }
        public Medium Medium { get; set; }
        public Condition Condition { get; set; }
        public ICollection<Description> Descriptions { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? RemoveDate { get; set; }
        public bool IsActive { get; set; }
    }
}