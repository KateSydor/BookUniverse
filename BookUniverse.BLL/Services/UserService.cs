namespace BookUniverse.BLL.Services
{
    using System.Threading.Tasks;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.UserRepository;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userService)
        {
            _userRepository = userService;
        }

        public async Task<User> GetUser(string email)
        {
            return await _userRepository.Get(u => u.Email == email);
        }
    }
}
