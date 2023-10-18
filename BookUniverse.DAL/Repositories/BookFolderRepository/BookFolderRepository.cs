namespace BookUniverse.DAL.Repositories.BookFolderRepository
{
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;

    public class BookFolderRepository : Repository<BookFolder>, IBookFolderRepository
    {
        public BookFolderRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
