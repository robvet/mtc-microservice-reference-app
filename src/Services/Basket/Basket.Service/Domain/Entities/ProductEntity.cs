using Microsoft.Build.Framework;
using System;
using System.Runtime.Serialization;

namespace Basket.Service.Domain.Entities
{
    public class ProductEntity
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Status { get; set; }
        public string Condition { get; set; }
        public string Medium { get; set; }
    }
}