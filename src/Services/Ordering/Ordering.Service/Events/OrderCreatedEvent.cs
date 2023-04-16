using EventBus.Events;

namespace order.service.Events
{
    public class OrderCreatedEvent : MessageEvent
    {
        public OrderInformationModel OrderInformationModel { get; set; }
    }
}