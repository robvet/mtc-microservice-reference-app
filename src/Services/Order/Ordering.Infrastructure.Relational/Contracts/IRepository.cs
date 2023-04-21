using System.Threading.Tasks;

namespace Ordering.Infrastructure.Relational.Contracts
{
    public interface IRelationalRepository<T>
    {
        Task<int> Add(T entity);
        Task Remove(T entity);
        Task Update(T entity);
        Task<int> Save();
    }
}