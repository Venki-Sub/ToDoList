namespace ToDoList;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// The design is a light pastel theme.
		UserAppTheme = AppTheme.Light;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}
