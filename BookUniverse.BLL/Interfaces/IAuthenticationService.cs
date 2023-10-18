using BookUniverse.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookUniverse.BLL.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> Login(string email, string password);
        User? CurrentAccount { get; set; }
        bool IsLoggedIn();
        void Logout();
    }
}
