

namespace ToDoList.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string dailyQuote = "The best way to predict the future is to create it.";

        [ObservableProperty]
        private string quoteAuthor = "- Peter Drucker MAUI";

        public MainPageViewModel()
        {
            Title = "Hello buddy!";
        }
        [RelayCommand]
        public async Task NavigateToDo()
        {
            // Navigate to the To-Do page using Shell navigation
            await Shell.Current.GoToAsync(nameof(ToDoItemPage));
            
        }
    }
}
