using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

namespace ToDoList;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				// Exo 2 – Google Fonts (SIL Open Font License)
				fonts.AddFont("Exo2-Regular.ttf", "Exo2Regular");
				fonts.AddFont("Exo2-Medium.ttf", "Exo2Medium");
				fonts.AddFont("Exo2-SemiBold.ttf", "Exo2SemiBold");
				fonts.AddFont("Exo2-Bold.ttf", "Exo2Bold");
				// Material Icons Round – Google (Apache 2.0)
				fonts.AddFont("MaterialIconsRound-Regular.otf", "MaterialIcons");
			});

		RemoveNativeInputChrome();

		// Services
		builder.Services.AddSingleton<IitemStore<TodoItem>, MockItemStore>();
		// Later, switch to an API backend:
		// builder.Services.AddSingleton<IitemStore<TodoItem>, ApiDataStore>();

		// ViewModels
		builder.Services.AddTransient<OnboardingViewModel>();
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<ToDoItemViewModel>();
		builder.Services.AddTransient<ToDoDetailViewModel>();
		builder.Services.AddTransient<NewToDoViewModel>();

		// Views
		builder.Services.AddTransient<OnboardingPage>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<ToDoItemPage>();
		builder.Services.AddTransient<ToDoDetailPage>();
		builder.Services.AddTransient<NewToDoPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	/// <summary>
	/// Inputs are wrapped in our own rounded "InputField" borders, so strip the
	/// platform border / underline to avoid a double outline.
	/// </summary>
	private static void RemoveNativeInputChrome()
	{
		EntryHandler.Mapper.AppendToMapping("NoChrome", (handler, view) =>
		{
#if ANDROID
			handler.PlatformView.BackgroundTintList =
				Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
			handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
			handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
		});

		EditorHandler.Mapper.AppendToMapping("NoChrome", (handler, view) =>
		{
#if ANDROID
			handler.PlatformView.BackgroundTintList =
				Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif WINDOWS
			handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
		});

		DatePickerHandler.Mapper.AppendToMapping("NoChrome", (handler, view) =>
		{
#if ANDROID
			handler.PlatformView.BackgroundTintList =
				Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
		});
	}
}
