using EventBus.Events;

namespace order.service.Events
{
    public class EmptyBasketEvent : MessageEvent
    {
        public string BasketID { get; set; }
    }
}