namespace order.infrastructure.nosql.Persistence.Contracts;

using Microsoft.Azure.Cosmos;

public interface ICosmosDbContainer
{
    Container Container { get; }
}
