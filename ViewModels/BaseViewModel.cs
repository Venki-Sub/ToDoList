namespace ToDoList.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    // Title for each page
    [ObservableProperty]
    private string title = string.Empty;

    // Shows loading spinner, disables UI actions
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;

    public BaseViewModel(string title = "")
    {
        Title = title;
    }

    protected static Task NavigateAsync(string route, bool animate = true)
        => Shell.Current.GoToAsync(route, animate);

    protected static Task GoBackAsync()
        => Shell.Current.GoToAsync("..", true);

    protected static Task AlertAsync(string title, string message)
        => Shell.Current.DisplayAlertAsync(title, message, "OK");

    protected static Task<bool> ConfirmAsync(string title, string message, string accept, string cancel = "Cancel")
        => Shell.Current.DisplayAlertAsync(title, message, accept, cancel);
}
