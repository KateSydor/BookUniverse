namespace BookUniverse.BLL.Services
{
    using AutoMapper;
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.BLL.Utils;
    using BookUniverse.DAL.Constants.UtilsConstants;
    using BookUniverse.DAL.Entities;
    using BookUniverse.DAL.Repositories.UserRepository;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;
        const string error = "Not valid credentials.";

        public AuthenticationService(IUserRepository userService, IMapper mapper, ILoggingService logger)
        {
            _userRepository = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public User? CurrentAccount
        {
            get; set;
        }

        public async Task Register(RegistrationDto user)
        {
            if (user.Password != user.RepeatPassword)
            {
                string errMsg = "Passwords don't match";
                _logger.LogError(user, errMsg);
                throw new ArgumentException(errMsg);
            }

            string storedHashedPassword = Hasher.ComputeHash(user.Password);

            User newUser = _mapper.Map<User>(user, opt => opt.Items["hashedPassword"] = storedHashedPassword);

            await _userRepository.Create(newUser);
            CurrentAccount = newUser;
            SerializeUser(CurrentAccount, UtilsConstants.FILE_PATH);
            _logger.LogInformation($"User {user.Username} successfully registered");
        }

        public async Task<User> Login(LoginDto user)
        {
            User storedAccount = await _userRepository.Get(u => u.Username == user.Username);

            if (storedAccount == null)
            {
                _logger.LogError(user, $"There is no user with username: {user.Username} in the database");
                throw new Exception(error);
            }

            string storedHashedPassword = Hasher.ComputeHash(user.Password);
            if (storedHashedPassword != storedAccount.Password)
            {
                _logger.LogError(user, error);
                throw new Exception(error);
            }

            CurrentAccount = storedAccount;
            SerializeUser(CurrentAccount, UtilsConstants.FILE_PATH);
            _logger.LogInformation($"User {user.Username} successfully logged in");
            return storedAccount;
        }

        public async Task EditUser(int userId, EditUserDto newUser)
        {
            if (userId == null)
            {
                string errMsg = "Id is null";
                _logger.LogError(userId, errMsg);
                throw new ArgumentNullException(errMsg);
            }

            User userToUpdate = await _userRepository.Get(u => u.Id == userId);
            if (newUser == null || userToUpdate == null)
            {
                _logger.LogError(newUser, $"Either newUser with username {newUser.Username} or userToUpdate with username {userToUpdate.Username} is null");
                throw new Exception(UtilsConstants.ERROR);
            }

            _mapper.Map(newUser, userToUpdate);

            await _userRepository.Update(userToUpdate);

            CurrentAccount = userToUpdate;
            SerializeUser(CurrentAccount, UtilsConstants.FILE_PATH);
            _logger.LogInformation($"User {newUser.Username}-{newUser.Email} has been successfully edited");
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
            _logger.LogInformation($"User {CurrentAccount.Username} is trying to logout");
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
                string errMsg = "Error during logout";
                _logger.LogError(null, errMsg);
                throw new Exception(errMsg);
            }
        }

        private void SerializeUser(User user, string filePath)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(filePath);
                writer.WriteLine(user.Id.ToString());
                writer.WriteLine(user.Email.ToString());
            }
            catch
            {
                string errMsg = "Error during sign in";
                _logger.LogError(null, errMsg);
                throw new Exception(errMsg);
            }
        }
    }
}
