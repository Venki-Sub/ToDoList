

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
                // Fire-and-forget the async loader so the property change doesn't block the UI thread.
                _ = LoadItemId(itemId);
            }
            else
            {
                Debug.WriteLine("Invalid ItemId provided");
            }
        }
        /// <summary>
        /// Loads a TodoItem from the store based on the provided itemId.
        /// If the item is found, it will update the corresponding properties of this view model.
        /// If the item is not found, it will write a debug message indicating the failure.
        /// </summary>
        /// <param name="itemId">The id of the item to load.</param>
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
