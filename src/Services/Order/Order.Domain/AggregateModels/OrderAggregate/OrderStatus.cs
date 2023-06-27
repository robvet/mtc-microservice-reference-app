using System.Collections.Generic;
using System.Text.Json.Serialization;
using order.domain.Contracts;

namespace order.domain.AggregateModels.OrderAggregate
{
    public class OrderStatus : IAggregateRoot
    {
        public OrderStatus()
        { }

        public OrderStatus(
            int statusId,
            string statusDescription)
        {
            StatusId = statusId;
            StatusDescription = statusDescription;
        }

        public int StatusId { get; private set; }
        public string StatusDescription { get; private set; }
        //[JsonIgnore]
        //public List<Order> Orders { get; private set; }
    }
}