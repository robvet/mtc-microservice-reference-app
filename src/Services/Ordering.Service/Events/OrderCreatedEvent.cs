using EventBus.Events;

namespace Ordering.API.Events
{
    public class OrderCreatedEvent : MessageEvent
    {
        public OrderInformationModel OrderInformationModel { get; set; }
    }
}