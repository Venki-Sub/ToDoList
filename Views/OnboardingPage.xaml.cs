namespace ToDoList.Views;

public partial class OnboardingPage : ContentPage
{
    public OnboardingPage(OnboardingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            Illustration.Opacity = 0;
            Illustration.TranslationY = 20;
            Runner.TranslationX = 40;
            await Task.WhenAll(
                Illustration.FadeToAsync(1, 500, Easing.CubicOut),
                Illustration.TranslateToAsync(0, 0, 500, Easing.CubicOut),
                Runner.TranslateToAsync(0, 0, 650, Easing.SpringOut));
        }
        catch
        {
            // animations are optional
        }
    }
}
