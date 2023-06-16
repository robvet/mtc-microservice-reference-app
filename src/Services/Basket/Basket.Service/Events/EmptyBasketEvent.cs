using EventBus.Events;
using System;

namespace Basket.Service.Events
{
    public class EmptyBasketEvent : MessageEvent

    {
        public Guid BasketID { get; set; }
    }
}