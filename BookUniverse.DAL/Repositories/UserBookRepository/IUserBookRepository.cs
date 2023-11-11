namespace BookUniverse.DAL.Repositories.UserBookRepository
{
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.Base;
	using System.Linq.Expressions;

    public interface IUserBookRepository : IRepository<UserBook>
    {
        IEnumerable<UserBook> GetAllByUser(Expression<Func<UserBook, bool>> filter);
	}
}
