namespace ToDoList.ViewModels;

public partial class OnboardingViewModel : BaseViewModel
{
    public const string HasOnboardedKey = "HasOnboarded";

    public OnboardingViewModel() : base("Welcome")
    {
    }

    [RelayCommand]
    private async Task GetStarted()
    {
        Preferences.Default.Set(HasOnboardedKey, true);
        await Shell.Current.GoToAsync("//MainPage", true);
    }
}
