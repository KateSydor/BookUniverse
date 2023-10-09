using BookUniverse.DAL.Persistence;
using BookUniverse.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookUniverse.DAL.Repositories.Base
{
    public class Repository<T> : IRepository<T>
         where T : class
    {
        private readonly DatabaseContext _db;
        internal DbSet<T> dbSet;

        public Repository(DatabaseContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task Create(T entity)
        {
            _db.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public async Task Update(T obj)
        {
            dbSet.Update(obj);
            await _db.SaveChangesAsync();
        }
    }
}
