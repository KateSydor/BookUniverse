using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Persistence;
using BookUniverse.DAL.Repositories.Base;

namespace BookUniverse.DAL.Repositories.UserBookRepository
{
    public class UserBookRepository : Repository<UserBook>, IUserBookRepository
    {
        public UserBookRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
