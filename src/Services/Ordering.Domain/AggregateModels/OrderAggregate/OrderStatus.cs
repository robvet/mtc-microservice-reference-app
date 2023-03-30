using System.Collections.Generic;
using Ordering.Domain.Contracts;

namespace Ordering.Domain.AggregateModels.OrderAggregate
{
    public class OrderStatus : IAggregateRoot
    {
        public OrderStatus()
        {}

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