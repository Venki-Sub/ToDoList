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
	}
}
