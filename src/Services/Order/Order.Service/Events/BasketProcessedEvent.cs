using EventBus.EventModels;
using EventBus.Events;


namespace order.service.Events
{
    public class BasketProcessedEvent : MessageEvent
    {
        public BasketProcessedEventModel basketProcessedEventModel { get; set; }
    }
}
