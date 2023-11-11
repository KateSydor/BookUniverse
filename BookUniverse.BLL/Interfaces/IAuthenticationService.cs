namespace BookUniverse.BLL.Interfaces
{
    using System.Threading.Tasks;
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.DAL.Entities;

    public interface IAuthenticationService
    {
        Task<User> Login(LoginDto user);

        Task Register(RegistrationDto user);

        User? CurrentAccount { get; set; }

        bool IsLoggedIn();

        Task EditUser(int userId, EditUserDto new_user);

        void Logout();
    }
}
