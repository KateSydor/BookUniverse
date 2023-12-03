namespace BookUniverse.BLL.Interfaces
{
    using BookUniverse.DAL.Entities;

    public interface IUserService
    {
        Task<User> GetUser(string email);
    }
}
