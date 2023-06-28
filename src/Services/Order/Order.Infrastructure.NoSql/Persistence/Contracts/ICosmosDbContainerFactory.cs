namespace order.infrastructure.nosql.Persistence.Contracts;

using System.Threading.Tasks;

public interface ICosmosDbContainerFactory
{
    /// <summary>
    ///     Returns a CosmosDbContainer wrapper
    /// </summary>
    /// <param name="containerName"></param>
    /// <returns></returns>
    ICosmosDbContainer GetContainer(string containerName);

    /// <summary>
    ///     Ensure the database is created
    /// </summary>
    /// <returns></returns>
    //Task EnsureDbSetupAsync();
}
