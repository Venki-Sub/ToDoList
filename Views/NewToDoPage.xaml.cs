using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class NewToDoPage : ContentPage
{
    public NewToDoPage(NewToDoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}
