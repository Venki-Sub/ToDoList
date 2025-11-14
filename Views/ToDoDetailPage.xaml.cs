

namespace ToDoList.Views
{
    public partial class ToDoDetailPage : ContentPage
    {
        public ToDoDetailPage(ToDoDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            
        }
    }
}