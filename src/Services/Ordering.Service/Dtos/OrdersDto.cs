using System;

namespace Ordering.API.Dtos
{
    public class OrdersDto
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CheckoutId { get; set; }
        public string BuyerName { get; set; }
        public string OrderId { get; set; }
        public string ShoppingBasketId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
    }
}