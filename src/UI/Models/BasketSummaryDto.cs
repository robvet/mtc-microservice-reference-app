namespace MusicStore.Models
{
    public class BasketSummaryDto
    {
        public string BasketId { get; set; }
        public string ProductNames { get; set; }
        public int ItemCount { get; set; }
        public string CheckoutId { get; set; }
        public string BuyerEmail { get; set; }
        public string CorrelationId { get; set; }
    }
}