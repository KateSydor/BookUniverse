using BookUniverse.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookUniverse.BLL.Interfaces
{
    public interface IAuthenticator
    {
        User? CurrentAccount { get; }
        bool IsLoggedIn { get; }
        Task<User> Login(string email, string password);

        void Logout();
    }
}
