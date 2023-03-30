using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class BasketDto
    {
        public BasketDto()
        {
            CartItems = new List<BasketItemDto>();
        }
        public string BasketId { get; set; }

        public List<BasketItemDto> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public int ItemCount { get; set; }

    }
}
