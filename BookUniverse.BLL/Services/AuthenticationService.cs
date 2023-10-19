namespace BookUniverse.BLL.Services
{
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Utils;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.UserRepository;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userService)
        {
            _userRepository = userService;
        }

        public User? CurrentAccount
        {
            get; set;
        }

        public async Task<User> Login(string userName, string password)
        {
            User storedAccount = await _userRepository.Get(u => u.Username == userName);

            if (storedAccount == null)
            {
                throw new Exception("Користувача з вказаною поштою не існує.");
            }

            string storedHashedPasssword = Hasher.ComputeHash(password);
            if (storedHashedPasssword != storedAccount.Password)
            {
                throw new Exception("Неправильний пароль.");
            }

            CurrentAccount = storedAccount;
            return storedAccount;
        }

        public bool IsLoggedIn()
        {
            if (CurrentAccount is null)
                return false;
            return true;
        }

        public void Logout()
        {
            CurrentAccount = null;
        }
    }
}
