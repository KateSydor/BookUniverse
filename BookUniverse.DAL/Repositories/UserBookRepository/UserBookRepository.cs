namespace BookUniverse.DAL.Repositories.UserBookRepository
{
    using System.Linq;
    using System.Linq.Expressions;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;

    public class UserBookRepository : Repository<UserBook>, IUserBookRepository
    {
        public UserBookRepository(DatabaseContext context)
        : base(context)
        {
        }

        public IEnumerable<Book> GetAllByUser(Expression<Func<UserBook, bool>> filter)
        {
            IQueryable<UserBook> query = dbSet;
            return query.Where(filter).Select(ub => ub.Book).ToList();
        }
    }
}
