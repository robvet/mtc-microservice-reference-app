using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Domain.Entities
{
    public class CheckoutEntity
    {
        public string CheckoutSystemId { get; set; }
        public string BuyerEmail { get; set; }
        public string CorrelationId { get; set; }
    }
}
