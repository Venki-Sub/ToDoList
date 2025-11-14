

namespace ToDoList.Views
{
    public partial class ToDoDetailPage : ContentPage
    {
        public ToDoDetailPage(ToDoDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch
            {
                // ignore navigation errors
            }
        }
    }
}