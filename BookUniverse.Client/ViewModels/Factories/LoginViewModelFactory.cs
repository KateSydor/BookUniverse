namespace BookUniverse.Client.ViewModels.Factories
{
    using BookUniverse.BLL.Interfaces;
    using BookUniverse.Client.ViewModels.Factories.Interfaces;

    public class LoginViewModelFactory : IBaseViewModelFactory<LoginViewModel>
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
