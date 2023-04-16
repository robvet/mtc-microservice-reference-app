using System.Threading.Tasks;
using EventBus.Events;

namespace EventBus.Bus
{
    public interface IEventBusPublisher
    {
        Task Publish<T>(MessageEvent messageEvent);
    }
}