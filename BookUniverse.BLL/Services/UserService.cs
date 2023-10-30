using BookUniverse.BLL.Interfaces;
using BookUniverse.DAL.Entities;
using BookUniverse.DAL.Repositories.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookUniverse.BLL.Services
{
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
