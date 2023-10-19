namespace BookUniverse.Client.ViewModels.Factories.Interfaces
{
    public interface IBaseViewModelFactory<T> where T : ViewModelBase
    {
        T CreateViewModel();
    }
}
