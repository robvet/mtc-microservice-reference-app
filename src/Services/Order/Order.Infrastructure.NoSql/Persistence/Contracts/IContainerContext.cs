namespace order.infrastructure.nosql.Persistence.Contracts;

using Microsoft.Azure.Cosmos;
using order.domain.AggregateModels;

public interface IContainerContext<T> where T : Item

{
    string ContainerName { get; }
    Task<string> GenerateId(T item);
    PartitionKey ResolvePartitionKey(string itemId);
}
