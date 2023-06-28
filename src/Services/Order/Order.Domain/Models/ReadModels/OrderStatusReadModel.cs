using System.Collections.Generic;
using System.Text.Json.Serialization;
using order.domain.Contracts;

namespace order.domain.Models.ReadModels
{
    public class OrderStatusReadModel
    {
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
    }
}