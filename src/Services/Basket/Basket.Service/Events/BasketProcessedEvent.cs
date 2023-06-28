using EventBus.Events;
using System;

namespace Basket.Service.Events
{
    public class BasketProcessedEvent : MessageEvent

    {
        public Guid BasketID { get; set; }
    }
}