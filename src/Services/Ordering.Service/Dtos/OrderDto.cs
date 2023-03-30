using System;
using System.Collections.Generic;

namespace Ordering.API.Dtos
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderDetails = new HashSet<OrderDetailDto>();
        }

        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShoppingBasketId { get; set; }
        public string Username { get; set; }
        public decimal Total { get; set; }

        public virtual ICollection<OrderDetailDto> OrderDetails { get; set; }
    }
}