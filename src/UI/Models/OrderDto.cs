using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class OrderDto
    {
        public string Id { get; set; }
        
        [DisplayName("Order Id")]
        public string OrderId { get; set; }
        
        [DisplayName("Order Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OrderDate { get; set; }
        
        [DisplayName("UserId")]
        [StringLength(25)]
        public string UserName { get; set; }

        [DisplayName("BuyerName")]
        [StringLength(25)]
        public string BuyerName { get; set; }

        [DisplayName("Checkout Id")]
        public string CheckoutId { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [DisplayName("Email")]
        [StringLength(40)]
        public string Email { get; set; }

        public virtual IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
