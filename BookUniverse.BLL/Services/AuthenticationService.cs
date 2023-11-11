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
        const string error = "Not valid credentials.";

        public AuthenticationService(IUserRepository userService, IMapper mapper)
        {
            _userRepository = userService;
            _mapper = mapper;
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

            string storedHashedPassword = Hasher.ComputeHash(user.Password);

            User newUser = _mapper.Map<User>(user, opt => opt.Items["hashedPassword"] = storedHashedPassword);

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

            string storedHashedPassword = Hasher.ComputeHash(user.Password);
            if (storedHashedPassword != storedAccount.Password)
            {
                throw new Exception(error);
            }

            CurrentAccount = storedAccount;
            SerializeUser(CurrentAccount, UtilsConstants.FILE_PATH);
            return storedAccount;
        }

        public async Task EditUser(int userId, EditUserDto newUser)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("Id is null");
            }

            User userToUpdate = await _userRepository.Get(u => u.Id == userId);
            if (newUser == null || userToUpdate == null)
            {
                throw new Exception(UtilsConstants.ERROR);
            }

            _mapper.Map(newUser, userToUpdate);

            await _userRepository.Update(userToUpdate);

            CurrentAccount = userToUpdate;
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
