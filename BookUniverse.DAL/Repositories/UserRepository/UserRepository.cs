namespace BookUniverse.DAL.Repositories.UserRepository
{
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
