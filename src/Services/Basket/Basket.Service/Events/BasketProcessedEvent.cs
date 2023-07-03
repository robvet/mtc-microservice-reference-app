using EventBus.EventModels;
using EventBus.Events;

namespace basket.service.Events
{
    public class BasketProcessedEvent : MessageEvent
    {
        public BasketProcessedEventModel basketProcessedEventModel { get; set; }
    }
}
