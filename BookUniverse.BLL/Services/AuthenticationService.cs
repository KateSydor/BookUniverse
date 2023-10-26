namespace BookUniverse.BLL.Services
{
    using BookUniverse.BLL.DTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Utils;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.UserRepository;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        const string error = "Not valid credentials.";

        public AuthenticationService(IUserRepository userService)
        {
            _userRepository = userService;
        }

        public User? CurrentAccount
        {
            get; set;
        }

        public async Task Register(RegistrationDto user)
        {
            if (user.Password != user.RepeatPassword)
            {
                throw new ArgumentException("Passwords don't match");
            }
            string storedHashedPasssword = Hasher.ComputeHash(user.Password);
            User newUser = new User
            {
                Username = user.Username,
                Email = user.Email,
                Password = storedHashedPasssword
            };

            await _userRepository.Create(newUser);

            CurrentAccount = newUser;
        }

        public async Task<User> Login(LoginDto user)
        {
            User storedAccount = await _userRepository.Get(u => u.Username == user.Username);

            if (storedAccount == null)
            {
                throw new Exception(error);
            }

            string storedHashedPasssword = Hasher.ComputeHash(user.Password);
            if (storedHashedPasssword != storedAccount.Password)
            {
                throw new Exception(error);
            }

            CurrentAccount = storedAccount;
            return storedAccount;
        }

        public bool IsLoggedIn()
        {
            if (CurrentAccount is null)
            {
                return false;
            }

            return true;
        }

        public void Logout()
        {
            CurrentAccount = null;
        }
    }
}
