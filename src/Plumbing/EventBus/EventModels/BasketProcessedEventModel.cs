using EventBus.Events;
using System;

namespace EventBus.EventModels
{
    public class BasketProcessedEventModel : MessageEvent
    {
        public Guid BasketID { get; set; }
    }
}