using EventBus.EventModels;
using EventBus.Events;

namespace Basket.Service.Events
{
    public class CheckOutEvent : MessageEvent
    {
        public CheckOutEventModel checkOutEventModel { get; set; }
    }
}