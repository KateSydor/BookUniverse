namespace BookUniverse.DAL.Repositories.UserBookRepository
{
    using System.Linq.Expressions;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.Base;

    public interface IUserBookRepository : IRepository<UserBook>
    {
        IEnumerable<Book> GetAllByUser(Expression<Func<UserBook, bool>> filter);
    }
}
