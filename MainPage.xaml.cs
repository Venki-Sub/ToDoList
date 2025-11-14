

namespace ToDoList;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.MainPageViewModel();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		try
		{
			// initial states
			HeroCard.Opacity = 0;
			HeroCard.TranslationY = 30;
			CtaButton.Scale = 0.9;

			// entrance animations (use non-obsolete async methods)
			await Task.WhenAll(
				HeroCard.FadeToAsync(1, 420, Easing.CubicOut),
				HeroCard.TranslateToAsync(0, 0, 420, Easing.CubicOut)
			);

			await CtaButton.ScaleToAsync(1, 280, Easing.SpringOut);
		}
		catch
		{
			// ignore animation errors on unsupported platforms
		}
	}
}
