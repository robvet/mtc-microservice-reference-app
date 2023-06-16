using System;
using System.Collections.Generic;

namespace Basket.Service.Domain.Entities
{
    public class BasketEntity
    {
        public Guid BasketId { get; set; }

        public string CorrelationToken { get; set; }

        public Guid BuyerId { get; set; }
                       
        public int Count { get; set; }

        public List<BasketItemEntity> Items { get; set; } = new List<BasketItemEntity>();
    }
}
