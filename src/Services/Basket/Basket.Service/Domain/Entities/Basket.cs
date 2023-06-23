using System;
using System.Collections.Generic;

namespace Basket.Service.Domain.Entities
{
    public class Basket
    {
        public Guid BasketId { get; set; }

        public string CorrelationToken { get; set; }

        public Guid BuyerId { get; set; }
                       
        public int ItemCount { get; set; }

        public bool Processed { get; set; }

        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
