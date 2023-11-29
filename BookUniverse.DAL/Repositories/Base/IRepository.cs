namespace BookUniverse.DAL.Repositories.Base
{
    using System.Linq.Expressions;

    public interface IRepository<T>
        where T : class
    {
        IEnumerable<T> GetAll();

        Task<T> Get(Expression<Func<T, bool>> filter);

        Task<T> Create(T entity);

        Task Delete(T entity);

        Task Update(T obj);
    }
}
