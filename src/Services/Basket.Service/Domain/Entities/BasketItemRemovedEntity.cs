namespace Basket.API.Domain.Entities
{
    public class BasketItemRemovedEntity
    {
        public string Message { get; set; }
        public decimal BasketTotal { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}