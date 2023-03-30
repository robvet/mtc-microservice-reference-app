namespace AdminTools.Database
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        int SaveChanges();
    }
}