using System;
using System.Collections.Generic;

namespace Basket.Service.Domain.Entities
{
    public class BasketEntity
    {
        public Guid BasketId { get; set; }

        public string CorrelationToken { get; set; }

        public Guid BuyerId { get; set; }
                       
        public int ItemCount { get; set; }

        public bool Processed { get; set; }

        public List<BasketItemEntity> Items { get; set; } = new List<BasketItemEntity>();
    }
}
