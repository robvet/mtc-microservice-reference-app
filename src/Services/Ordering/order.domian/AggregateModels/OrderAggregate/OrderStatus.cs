using System.Collections.Generic;
using order.domian.Contracts;

namespace order.domian.AggregateModels.OrderAggregate
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

        public int Id { get; private set; }
        public int StatusId { get; private set; }
        public string StatusDescription { get; private set; }
        public List<Order> Orders { get; private set; }
    }
}