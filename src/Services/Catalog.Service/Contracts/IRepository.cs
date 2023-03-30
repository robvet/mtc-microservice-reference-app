using System.Threading.Tasks;

namespace Catalog.API.Contracts
{
    public interface IRepository<T>
    {
        Task Add(T entity);
        Task Remove(T entity);
        Task Update(T entity);
        Task<int> Save();
    }
}