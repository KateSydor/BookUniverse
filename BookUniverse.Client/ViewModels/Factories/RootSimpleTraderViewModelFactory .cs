using System;

namespace BookUniverse.Client.ViewModels.Factories
{
    public class RootSimpleTraderViewModelFactory : IRootSimpleTraderViewModelFactory
    {
        private readonly ISimpleTraderViewModelFactory<LoginViewModel> _loginViewModelFactory;

        public RootSimpleTraderViewModelFactory(ISimpleTraderViewModelFactory<LoginViewModel> loginViewModelFactory)
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
