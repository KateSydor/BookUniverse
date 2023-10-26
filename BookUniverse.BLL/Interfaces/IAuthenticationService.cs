namespace BookUniverse.BLL.Interfaces
{
    using System.Threading.Tasks;
    using BookUniverse.DAL.Entities;

    public interface IAuthenticationService
    {
        Task<User> Login(string userName, string password);

        Task Register(string userName, string email, string password, string repeatPassword);

        User? CurrentAccount { get; set; }

        bool IsLoggedIn();

        void Logout();
    }
}
