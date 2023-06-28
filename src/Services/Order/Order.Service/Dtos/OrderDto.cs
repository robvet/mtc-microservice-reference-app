using System;
using System.Collections.Generic;

namespace order.service.Dtos
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderDetails = new HashSet<OrderDetailDto>();
        }

        public string id { get; set; }
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid ShoppingBasketId { get; set; }
        public string Username { get; set; }
        public Guid CustomerId { get; set; }
        public string BuyerName { get; set; }
        public decimal Total { get; set; }

        public virtual ICollection<OrderDetailDto> OrderDetails { get; set; }
    }
}