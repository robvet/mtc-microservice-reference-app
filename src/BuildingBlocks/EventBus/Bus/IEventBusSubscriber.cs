using EventBus.Events;

namespace EventBus.Bus
{
    public interface IEventBusSubscriber
    {
        void Subscribe<T>(IMessageEventHandler messageHandler);
    }
}