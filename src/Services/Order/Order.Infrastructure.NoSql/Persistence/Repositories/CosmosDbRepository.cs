namespace order.infrastructure.nosql.Persistence.Repositories;

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using order.domain.AggregateModels;
using order.infrastructure.nosql.Persistence.Contracts;

public abstract class CosmosDbRepository<T> : IRepository<T>, IContainerContext<T> where T : Item
{
    public abstract string ContainerName { get; }
    private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
    private readonly Container _container;

    public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
    {
        _cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
        _container = _cosmosDbContainerFactory.GetContainer(ContainerName).Container;
    }
    public async Task<T> AddAsync(T item)
    {
        try
        {
            //var response = await _container.CreateItemAsync(item, ResolvePartitionKey(item.Id));
            var response = await _container.CreateItemAsync(item, ResolvePartitionKey(item.Id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            await Console.Out.WriteLineAsync(ex.Message);
        }

        return default;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            await _container.DeleteItemAsync<T>(id, ResolvePartitionKey(id));
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public virtual async Task<string> GenerateId(T item)
    {
        var queryDefinition = "SELECT VALUE COUNT(1) FROM c";
        var iterator = _container.GetItemQueryIterator<int>(queryDefinition);
        var result = await iterator.ReadNextAsync();
        var count = result.First();
        return (count + 1).ToString();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        try
        {
            //**************************************************
            //* This requires understanding!
            //*
            //* The partition key is not the same as the id
            //* The id refers to the 'id' property of the item
            //* The partition key refers to the 'partition key' property of the item, 
            //* which in our case is /OrderId
            //* These are different properties
            //* To search by id, you must use the id property
            //* Which makes sense, but is not intuitive
            //* The paritition key can never be guanteed to be unique across all items
            //* The partition key is the first part of the id
            //* The id is the entire id
            //* The partition key is the first part of the id
            //*  https://stackoverflow.com/questions/60118213/azure-cosmos-db-delete-ids-definitely-exist/60149506#60149506
            //**************************************************


            // This works because passing separate values for id and partition key
            //var response = await _container.ReadItemAsync<T>("47c01144-b495-4c8a-a609-6254aff04f8b", ResolvePartitionKey(id));
                    
            var response = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
            return response.Resource;


            //// temp
            //var part = ResolvePartitionKey(id);
            ////var response = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    // The ResolvePartitionKey method requires the entire item object to be passed in to properly resolve the partition key value.
    // Without the full item object, the method cannot access the necessary properties to lookup and return the partition key.
    //public virtual PartitionKey ResolvePartitionKey(T item) => new PartitionKey(item.OrderId);
    //public virtual PartitionKey ResolvePartitionKey(string itemId) => new PartitionKey(itemId);
    public virtual PartitionKey ResolvePartitionKey(string itemId) => new PartitionKey(itemId.Split(':')[0]);

    public async Task<T> UpdateAsync(string id, T item)
    {
        try
        {
            item.Id = id;
            //item.OrderId = id;
            var response = await _container.ReplaceItemAsync(item, id);
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task<IEnumerable<T>> GetItemsAsync()
    {
        return await GetItemsAsync("SELECT * FROM c");
    }

    protected async Task<IEnumerable<T>> GetItemsAsync(string query)
    {
        FeedIterator<T> resultSetIterator = _container.GetItemQueryIterator<T>(new QueryDefinition(query));
        List<T> results = new List<T>();
        while (resultSetIterator.HasMoreResults)
        {
            FeedResponse<T> response = await resultSetIterator.ReadNextAsync();

            results.AddRange(response.ToList());
        }

        return results;
    }

    protected async Task<TValue> GetScalarValueAsync<TValue>(string query)
    {
        FeedIterator<TValue> resultSetIterator = _container.GetItemQueryIterator<TValue>(new QueryDefinition(query));
        while (resultSetIterator.HasMoreResults)
        {
            return (await resultSetIterator.ReadNextAsync()).SingleOrDefault();
        }

        return default;
    }
}
