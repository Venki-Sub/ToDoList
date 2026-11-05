namespace ToDoList.Views;

public partial class NewToDoPage : ContentPage
{
    private readonly NewToDoViewModel viewModel;

    public NewToDoPage(NewToDoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Jump straight into typing when creating a new task.
        if (!viewModel.IsEditMode)
        {
            Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(350), () => NameEntry.Focus());
        }
    }
}
