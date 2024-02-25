namespace lab2.ViewModels;

public class MainViewModel : ViewModelBase
{
    public FilterViewModel Filter { get; init; }
    public StoreViewModel Store { get; init; }

    public MainViewModel()
    {
        Store = new();
        Filter = new(Store.Toys);
    }

}
