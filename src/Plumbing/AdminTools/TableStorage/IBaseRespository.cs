using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AdminTools.TableStorage
{
    public interface IBaseRespository<T> where T : TableEntity, new()
    {
        Task Delete(string partitionKey, string rowKey);
        Task Delete(List<T> items, string correlationToken);
        Task<T> GetItem(string partitionKey, string rowKey);
        Task<List<T>> GetList();
        Task<List<T>> GetList(string partitionKey);
        Task Insert(T item, string correlationToken);
        Task Insert(List<T> items, string correlationToken);
        Task Update(T item, string correlationToken);
    }
}