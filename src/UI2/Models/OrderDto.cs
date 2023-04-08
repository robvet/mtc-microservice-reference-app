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
        
        [DisplayName("User")]
        [StringLength(25)]
        public string BuyerName { get; set; }
        
        [DisplayName("Checkout Id")]
        public string CheckoutId { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

    }
}
