using System.Threading.Tasks;

namespace catalog.service.Contracts
{
    public interface IDataSeedingServices
    {
        Task SeedDatabase(string correlationToken);
    }
}