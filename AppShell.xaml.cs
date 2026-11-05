namespace ToDoList;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes for navigation
		Routing.RegisterRoute(nameof(ToDoItemPage), typeof(ToDoItemPage));
		Routing.RegisterRoute(nameof(ToDoDetailPage), typeof(ToDoDetailPage));
		Routing.RegisterRoute(nameof(NewToDoPage), typeof(NewToDoPage));

		// Skip the welcome screen once the user has tapped "Get Started".
		if (Preferences.Default.Get(OnboardingViewModel.HasOnboardedKey, false))
		{
			CurrentItem = HomeContent;
		}
	}
}
