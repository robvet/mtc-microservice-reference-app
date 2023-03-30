using System.Threading.Tasks;
using CommandBus.Commands;

namespace CommandBus.Bus
{
    public interface ICommandBus
    {
        Task Send<T>(Command payload);

        Task<Command> Consume();

        void Subscribe<T>(ICommandHandler commandHandler);
    }
}
