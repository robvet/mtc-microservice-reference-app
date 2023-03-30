using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Basket.API.Domain.Entities
{
    /// <summary>
    /// Represents a Denormalized Read Table for ProductEntity Information
    /// </summary>
    public class ProductTableEntity : TableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public bool ParentalCaution { get; set; }
        //public string Upc { get; set; }
        //public DateTime? ReleaseDate { get; set; }
        public string Price { get; set; }
        //public bool? Cutout { get; set; }
        public string ArtistName { get; set; }
        public string GenreName { get; set; }
    }
}