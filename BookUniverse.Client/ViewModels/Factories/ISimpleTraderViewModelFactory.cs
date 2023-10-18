namespace BookUniverse.Client.ViewModels.Factories
{
    public interface ISimpleTraderViewModelFactory<T> where T : ViewModelBase
    {
        T CreateViewModel();
    }
}
