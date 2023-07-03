using System;

namespace order.service.Dtos
{
    public class OrdersDto
    {
        public string Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ShoppingBasketId { get; set; }
        public string BuyerName { get; set; }
        public string UserName { get; set; }
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Email { get; set; }
    }
}