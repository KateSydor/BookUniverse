namespace BookUniverse.BLL.Interfaces
{
    using System.Threading.Tasks;
    using BookUniverse.DAL.Entities;

    public interface IAuthenticationService
    {
        Task<User> Login(string email, string password);

        User? CurrentAccount { get; set; }

        bool IsLoggedIn();

        void Logout();
    }
}
