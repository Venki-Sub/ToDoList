namespace ToDoList.Views
{
    public partial class ToDoItemPage : ContentPage
    {
        public ToDoItemPage( ToDoItemViewModel viewModel )
        {
            InitializeComponent();
            BindingContext = viewModel; 
            // ViewModel for the To-Do list
        }
    }
}