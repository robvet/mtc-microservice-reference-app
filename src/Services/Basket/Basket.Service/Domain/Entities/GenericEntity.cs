using Microsoft.Build.Framework;
using System;
using System.Runtime.Serialization;

namespace Basket.Service.Domain.Entities
{
    public class GenericEntity
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
                
        public Guid BasketId { get; set; }
        public bool Processed { get; set; }
        public int ItemCount { get; set; }
    }
}