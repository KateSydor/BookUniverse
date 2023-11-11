namespace BookUniverse.DAL.Repositories.UserBookRepository
{
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;
	using System.Linq;
	using System.Linq.Expressions;

	public class UserBookRepository : Repository<UserBook>, IUserBookRepository
    {
        public UserBookRepository(DatabaseContext context)
        : base(context)
        {
        }

		public IEnumerable<UserBook> GetAllByUser(Expression<Func<UserBook, bool>> filter)
		{
			IQueryable<UserBook> query = dbSet;
			query = query.Where(filter);
			return query.ToList();
		}
	}
}
