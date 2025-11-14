using Microsoft.Extensions.Logging;

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
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		
		// Services
        builder.Services.AddSingleton<IitemStore<TodoItem>, MockItemStore>();
		//And if later you switch to an API backend:
		// builder.Services.AddSingleton<IItemStore, ApiDataStore>();


		// ViewModels
		builder.Services.AddTransient<ToDoItemViewModel>();
		builder.Services.AddTransient<ToDoDetailViewModel>();
		builder.Services.AddTransient<NewToDoViewModel>();

		// Views
		builder.Services.AddTransient<ToDoItemPage>();
		builder.Services.AddTransient<ToDoDetailPage>();
		builder.Services.AddTransient<NewToDoPage>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
