using System;

namespace Basket.API.Domain.Entities
{
    public class BasketItemEntity 
    {
        public int BasketParentId { get; set; }
        public string CorrelationToken { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
