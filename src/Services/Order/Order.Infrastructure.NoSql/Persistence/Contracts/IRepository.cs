namespace order.infrastructure.nosql.Persistence.Contracts;

using order.domain.Models;
using System.Threading.Tasks;

public interface IRepository<T> where T : Item

{
    Task<T> GetByIdAsync(string id);
    Task<T> AddAsync(T item);
    Task<T> UpsertAsync(T item);
    Task<T> UpdateAsync(string id, T item);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<T>> GetItemsAsync();
}
