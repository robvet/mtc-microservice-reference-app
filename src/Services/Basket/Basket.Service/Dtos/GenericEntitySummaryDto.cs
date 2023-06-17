using System;

namespace Basket.Service.Dtos
{
    public class GenericEntitySummaryDto
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }

        public Guid BasketId { get; set; }
        public bool Processed { get; set; }
        public int ItemCount { get; set; }
    }
}