using BookUniverse.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookUniverse.Client.ViewModels.Factories
{
    public class LoginViewModelFactory : ISimpleTraderViewModelFactory<LoginViewModel>
    {
        private readonly IAuthenticator _authenticator;

        public LoginViewModelFactory(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public LoginViewModel CreateViewModel()
        {
            return new LoginViewModel(_authenticator);
        }
    }
}
