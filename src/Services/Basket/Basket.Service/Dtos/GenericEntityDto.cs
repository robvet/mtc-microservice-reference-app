using System;
using System.Collections.Generic;

namespace Basket.Service.Dtos
{
    public class GenericEntityDto
    {
        public GenericEntityDto()
        {
            Baskets = new List<GenericEntitySummaryDto>();
            Products = new List<GenericEntitySummaryDto>();
        }
        public List<GenericEntitySummaryDto> Baskets{ get; set;}
        public List<GenericEntitySummaryDto> Products { get; set; }
    }
}