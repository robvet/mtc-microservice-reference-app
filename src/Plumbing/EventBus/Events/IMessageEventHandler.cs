using System.Threading.Tasks;

namespace EventBus.Events
{
    public interface IMessageEventHandler
    {
        Task HandleAsync(MessageEvent message);
    }
}