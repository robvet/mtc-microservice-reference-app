using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.API.Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace Basket.API.Infrastructure.Repository
{
    public class AzureTableStorageRespository<T> : IAzureTableStorageRespository<T>
        where T : TableEntity, new()
    {
        private readonly AzureTableSettings _settings;

        public AzureTableStorageRespository(AzureTableSettings settings)
        {
            _settings = settings;
        }

        public async Task<List<T>> GetList(string correlationToken)
        {
            //Table
            var table = await GetTableAsync();

            //Query
            var query = new TableQuery<T>();

            var results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                var queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);
            } while (continuationToken != null);

            return results;
        }

        public async Task<List<T>> GetList(string partitionKey, string correlationToken)
        {
            //Table
            var table = await GetTableAsync();

            //Query
            var query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, partitionKey));

            var results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                var queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;

                results.AddRange(queryResults.Results);
            } while (continuationToken != null);

            return results;
        }

        public async Task<T> GetItem(string partitionKey, string rowKey, string correlationToken)
        {
            
            //Table
            var table = await GetTableAsync();

            //Operation
            var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            //Execute
            var result = await table.ExecuteAsync(operation);

            return (T) result.Result;
        }

        public async Task Insert(T item, string correlationToken)
        {
            //Table
            var table = await GetTableAsync();

            //Operation
            //var operation = TableOperation.Insert(item);
            var operation = TableOperation.InsertOrReplace(item);

            //Execute
            await table.ExecuteAsync(operation);
        }

        public async Task Insert(List<T> items, string correlationToken)
        {
            var tableBatchOperation = new TableBatchOperation();

            var table = await GetTableAsync();

            foreach (var item in items)
            {
                tableBatchOperation.Insert(item, false);
            }

            await table.ExecuteBatchAsync(tableBatchOperation);
        }

        public async Task Update(T item, string correlationToken)
        {
            //Table
            var table = await GetTableAsync();

            //Operation
            var operation = TableOperation.InsertOrReplace(item);

            //Execute
            await table.ExecuteAsync(operation);
        }

        public async Task Delete(string partitionKey, string rowKey, string correlationToken)
        {
            //Title
            var item = await GetItem(partitionKey, rowKey, correlationToken);

            //Table
            var table = await GetTableAsync();

            //Operation
            var operation = TableOperation.Delete(item);

            //Execute
            await table.ExecuteAsync(operation);
        }


        private async Task<CloudTable> GetTableAsync()
        {
            //Account
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(_settings.StorageAccount.ToLower(), _settings.StorageKey), false);

            //Client
            var tableClient = storageAccount.CreateCloudTableClient();

            //Table
            var table = tableClient.GetTableReference(_settings.TableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}