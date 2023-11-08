namespace BookUniverse.BLL.Services
{
    using BookUniverse.BLL.DTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Utils;
    using BookUniverse.DAL.Constants.UtilsConstants;
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
                Password = storedHashedPasssword,
            };

            await _userRepository.Create(newUser);

            CurrentAccount = newUser;
            SerializeUser(CurrentAccount, UtilsConstants.FILE_PATH);
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
            SerializeUser(CurrentAccount, UtilsConstants.FILE_PATH);
            return storedAccount;
        }

        public async Task EditUser(EditUserDto new_user)
        {
            User newUser = new User
            {
                Username = new_user.Username,
                Email = new_user.Email,
                Password = CurrentAccount.Password,
            };

            await _userRepository.Create(newUser);

            CurrentAccount = newUser;
            SerializeUser(CurrentAccount, UtilsConstants.FILE_PATH);
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
            ClearAuthFile(UtilsConstants.FILE_PATH);
        }

        private void ClearAuthFile(string filePath)
        {
            try
            {
                File.WriteAllText(filePath, string.Empty);
            }
            catch
            {
                throw new Exception("Error during logout");
            }
        }

        private void SerializeUser(User user, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(user.Id.ToString());
                    writer.WriteLine(user.Email.ToString());
                }
            }
            catch
            {
                throw new Exception("Error during sign in");
            }
        }
    }
}
