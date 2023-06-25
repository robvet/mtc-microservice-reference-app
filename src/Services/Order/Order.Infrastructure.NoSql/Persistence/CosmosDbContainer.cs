namespace order.infrastructure.nosql.Persistence;

using Microsoft.Azure.Cosmos;
using order.infrastructure.nosql.Persistence.Contracts;

public class CosmosDbContainer : ICosmosDbContainer
{
    public Container Container { get; }

    public CosmosDbContainer(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        Container = cosmosClient.GetContainer(databaseName, containerName);
    }
}
