using BookUniverse.Client.ViewModels.Factories.Enums;

namespace BookUniverse.Client.ViewModels.Factories.Interfaces
{
    public interface IRootViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
