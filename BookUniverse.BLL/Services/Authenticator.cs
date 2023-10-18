using BookUniverse.BLL.Interfaces;
using BookUniverse.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookUniverse.BLL.Services
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;

        public Authenticator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public User CurrentAccount
        {
            get => _authenticationService.CurrentAccount!;
            set { _authenticationService.CurrentAccount = value!; }
        }

        public bool IsLoggedIn => _authenticationService.IsLoggedIn();

        public async Task<User> Login(string email, string password)
        {
            return await _authenticationService.Login(email, password);
        }

        public void Logout()
        {
            _authenticationService.Logout();
        }
    }
}
