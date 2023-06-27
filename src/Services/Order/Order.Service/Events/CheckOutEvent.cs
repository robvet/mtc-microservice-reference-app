using EventBus.Events;


namespace order.service.Events
{
    public class CheckOutEvent : MessageEvent
    {
        public OrderInformationModel OrderInformationModel { get; set; }
    }
}
