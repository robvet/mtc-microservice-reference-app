using EventBus.Events;
using System;

namespace order.service.Events
{
    public class BasketProcessedEvent : MessageEvent
    {
        public Guid BasketID { get; set; }
    }
}