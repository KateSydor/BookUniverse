namespace BookUniverse.DAL.Repositories.BookRepository
{
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;

    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
