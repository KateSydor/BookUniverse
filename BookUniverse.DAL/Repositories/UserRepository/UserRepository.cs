using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Persistence;
using BookUniverse.DAL.Repositories.Base;

namespace BookUniverse.DAL.Repositories.UserRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
