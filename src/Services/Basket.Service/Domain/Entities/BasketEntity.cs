using System.Collections.Generic;

namespace Basket.API.Domain.Entities
{
    public class BasketEntity
    {
        public BasketEntity()
        {}
        public BasketEntity(string customerId)
        {
            BuyerId = customerId;
        }

        public string CorrelationToken { get; set; }

        public string BuyerId { get; set; }

        public string BasketId { get; set; }
        
        public int Count { get; set; }

        public List<BasketItemEntity> Items { get; set; } = new List<BasketItemEntity>();
    }
}
