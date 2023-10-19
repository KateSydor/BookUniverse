using System;
using BookUniverse.Client.ViewModels.Factories.Enums;
using BookUniverse.Client.ViewModels.Factories.Interfaces;

namespace BookUniverse.Client.ViewModels.Factories
{
    public class RootViewModelFactory : IRootViewModelFactory
    {
        private readonly IBaseViewModelFactory<LoginViewModel> _loginViewModelFactory;

        public RootViewModelFactory(IBaseViewModelFactory<LoginViewModel> loginViewModelFactory)
        {
            _loginViewModelFactory = loginViewModelFactory;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    return _loginViewModelFactory.CreateViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
