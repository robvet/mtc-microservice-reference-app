using System;
using System.Collections.Generic;

namespace Basket.Service.Dtos
{
    public class GenericDto
    {
        public GenericDto()
        {
            Baskets = new List<GenericSummaryDto>();
            Products = new List<GenericSummaryDto>();
        }
        public List<GenericSummaryDto> Baskets{ get; set;}
        public List<GenericSummaryDto> Products { get; set; }
    }
}