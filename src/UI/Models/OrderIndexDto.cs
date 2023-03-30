using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class OrderIndexDto
    {
        public string OrderId { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        public string CheckoutId { get; set; }
        [DisplayName("User")]
        public string Username { get; set; }
        public decimal Total { get; set; }

        public virtual IEnumerable<OrderDetailDto> OrderDetails { get; set; }

    }
}
