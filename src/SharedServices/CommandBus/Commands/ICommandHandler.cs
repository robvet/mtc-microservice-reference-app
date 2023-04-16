using System.Threading.Tasks;

namespace CommandBus.Commands
{
    public interface ICommandHandler
    {
        Task HandleAsync(Command command);
    }
}