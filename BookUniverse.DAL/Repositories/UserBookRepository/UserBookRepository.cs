namespace BookUniverse.DAL.Repositories.UserBookRepository
{
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Persistence;
    using BookUniverse.DAL.Repositories.Base;

    public class UserBookRepository : Repository<UserBook>, IUserBookRepository
    {
        public UserBookRepository(DatabaseContext context)
        : base(context)
        {
        }
    }
}
