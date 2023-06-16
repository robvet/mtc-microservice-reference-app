using System;
using System.Collections.Generic;

namespace Basket.Service.Dtos
{
    public class BasketDto
    {
        public BasketDto()
        {
            CartItems = new List<BasketItemDto>();
        }

        public Guid BasketId { get; set; }

        public List<BasketItemDto> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public int ItemCount { get; set; }
    }
}