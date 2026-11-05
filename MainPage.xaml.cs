namespace ToDoList;

public partial class MainPage : ContentPage
{
	private readonly MainPageViewModel viewModel;

	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// Refresh every time we come back (task added / completed / deleted / pinned).
		await viewModel.RefreshAsync();
	}
}
