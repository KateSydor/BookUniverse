using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Persistence;
using BookUniverse.DAL.Repositories.Base;

namespace BookUniverse.DAL.Repositories.BookFolderRepository
{
    public class BookFolderRepository : Repository<BookFolder>, IBookFolderRepository
    {
        public BookFolderRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
