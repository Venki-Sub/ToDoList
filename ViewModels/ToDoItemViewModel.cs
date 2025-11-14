




namespace ToDoList.ViewModels
{
    public partial class ToDoItemViewModel : BaseViewModel
    {
      

    private readonly IitemStore<TodoItem> itemStore;

    public ObservableCollection<TodoItem> ToDoItems { get; } = new();

    public ToDoItemViewModel(IitemStore<TodoItem> itemStore) //, INavigationService navigation)
        : base("Ma ToDoList")
    {
        this.itemStore = itemStore;
          _ = LoadToDoItems();
    }

         // Load To-Do items from the store
        [RelayCommand]
        private async Task LoadToDoItems()
        {
            try
            {
                var items = await this.itemStore.GetItemsAsync();
                ToDoItems.Clear();
                foreach (var item in items)
                {
                    ToDoItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading items: {ex.Message}");
            }
            
        }

         [RelayCommand]
        private async Task NavigateToDetail(TodoItem tappedItem)
        {
            if (tappedItem != null)
            {
               //Pass the ID of the tapped item to the detail page
                await Shell.Current.GoToAsync($"{nameof(ToDoDetailPage)}?{nameof(ToDoDetailViewModel.ItemId)}={tappedItem.IdToDoItem}");
            }
        }
      
    }
}
