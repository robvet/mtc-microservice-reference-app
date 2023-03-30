using EventBus.Events;

namespace Ordering.API.Events
{
    public class EmptyBasketEvent : MessageEvent
    {
        public string BasketID { get; set; }
    }
}