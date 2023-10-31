using BookUniverse.DAL.Entities;

namespace BookUniverse.BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(string email);
    }
}
