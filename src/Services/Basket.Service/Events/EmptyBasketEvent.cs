using EventBus.Events;

namespace Basket.API.Events
{
    public class EmptyBasketEvent : MessageEvent

    {
        public string BasketID { get; set; }
    }
}