using System;

namespace Basket.Service.Dtos
{
    public class BasketSummaryDto
    {
        public Guid BasketId { get; set; }
        public string ProductNames { get; set; }
        public int ItemCount { get; set; }
        public bool Processed { get; set; }
        public string CheckoutId { get; set; }
        public string BuyerEmail { get; set; }
        public string CorrelationId { get; set; }
    }
}