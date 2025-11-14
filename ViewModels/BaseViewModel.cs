

namespace ToDoList.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    // Title for each page
    [ObservableProperty]
    private string title;

    // Shows loading spinner, disables UI actions
    [ObservableProperty]
    private bool isBusy;

    // Useful convenience property
    public bool IsNotBusy => !IsBusy;

    public BaseViewModel(string title = "")
    {
        Title = title;
    }
    
}
