using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Service.Domain.Entities
{
    public class Checkout
    {
        public Guid CheckoutSystemId { get; set; }
        public string BuyerEmail { get; set; }
        public string CorrelationId { get; set; }
    }
}
