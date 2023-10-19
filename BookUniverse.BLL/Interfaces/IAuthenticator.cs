namespace BookUniverse.BLL.Interfaces
{
    using BookUniverse.DAL.Entities;

    public interface IAuthenticator
    {
        User? CurrentAccount { get; }

        bool IsLoggedIn { get; }

        Task<User> Login(string userName, string password);

        void Logout();
    }
}
