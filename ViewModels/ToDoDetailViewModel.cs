

namespace ToDoList.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class ToDoDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? itemId;

        [ObservableProperty]
        private TodoItem toDoItem;
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string taskName;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private bool isCompleted;

        [ObservableProperty]
        private DateTime? dueDate;

        [ObservableProperty]
        private string priority;


    private readonly IitemStore<TodoItem> itemStore;

        public ToDoDetailViewModel(IitemStore<TodoItem> itemStore)
        {
            this.itemStore = itemStore;
        }

         partial void OnItemIdChanged(string? value)
        {
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int itemId))
            {
                LoadItemId(itemId).ConfigureAwait(false); // Pass the parsed int
            }
            else
            {
                Debug.WriteLine("Invalid ItemId provided");
            }
        }
        private async Task LoadItemId(int itemId)
        {
            try
            {
                var item = await this.itemStore.GetItemAsync(itemId);
               if (item != null)
                {
                    Id = item.IdToDoItem;
                    TaskName = item.ItemName;
                    Description = item.Description;
                    IsCompleted = item.IsCompleted;
                    DueDate = item.DueDate;
                    Priority = item.Priority;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to load item");
            }
        }
    }
}
