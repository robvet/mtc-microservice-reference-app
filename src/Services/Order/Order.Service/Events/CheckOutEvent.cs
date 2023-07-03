using EventBus.EventModels;
using EventBus.Events;


namespace order.service.Events
{
    public class CheckOutEvent : MessageEvent
    {
        public CheckOutEventModel checkOutEventModel { get; set; }
    }
}
