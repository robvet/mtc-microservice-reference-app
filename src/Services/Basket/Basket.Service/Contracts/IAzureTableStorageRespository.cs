using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Contracts
{
    public interface IAzureTableStorageRespository<T> where T : TableEntity, new()
    {
        Task Delete(string partitionKey, string rowKey, string correlationToken);
        Task<T> GetItem(string partitionKey, string rowKey, string correlationToken);
        Task<List<T>> GetList(string correlationToken);
        Task<List<T>> GetList(string partitionKey, string correlationToken);
        Task Insert(T item, string correlationToken);
        Task Insert(List<T> items, string correlationToken);
        Task Update(T item, string correlationToken);
    }
}