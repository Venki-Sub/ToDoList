namespace ToDoList.ViewModels
{
    public partial class ToDoItemViewModel : BaseViewModel
    {
        public const string FilterAll = "All";
        public const string FilterActive = "Active";
        public const string FilterDone = "Done";

        private readonly IitemStore<TodoItem> itemStore;
        private List<TodoItem> allItems = new();

        public ToDoItemViewModel(IitemStore<TodoItem> itemStore) : base("My Tasks")
        {
            this.itemStore = itemStore;
        }

        public ObservableCollection<TodoItem> ToDoItems { get; } = new();

        [ObservableProperty] private string filter = FilterAll;
        [ObservableProperty] private string todayText = DateTime.Today.ToString("dddd, MMMM d");
        [ObservableProperty] private int allCount;
        [ObservableProperty] private int activeCount;
        [ObservableProperty] private int doneCount;
        [ObservableProperty] private string emptyTitle = string.Empty;
        [ObservableProperty] private string emptyMessage = string.Empty;

        // Load To-Do items from the store
        [RelayCommand]
        private async Task LoadToDoItems()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                TodayText = DateTime.Today.ToString("dddd, MMMM d");
                allItems = (await itemStore.GetItemsAsync()).ToList();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading items: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        partial void OnFilterChanged(string value) => ApplyFilter();

        private void ApplyFilter()
        {
            AllCount = allItems.Count;
            DoneCount = allItems.Count(i => i.IsCompleted);
            ActiveCount = AllCount - DoneCount;

            IEnumerable<TodoItem> query = Filter switch
            {
                FilterActive => allItems.Where(i => !i.IsCompleted),
                FilterDone => allItems.Where(i => i.IsCompleted),
                _ => allItems
            };

            // Open tasks first, then by date and start time.
            var sorted = query
                .OrderBy(i => i.IsCompleted)
                .ThenBy(i => i.StartsAt ?? DateTime.MaxValue)
                .ToList();

            ToDoItems.Clear();
            foreach (var item in sorted)
            {
                ToDoItems.Add(item);
            }

            (EmptyTitle, EmptyMessage) = Filter switch
            {
                FilterActive => ("All caught up 🎉", "You have no open tasks. Enjoy your day!"),
                FilterDone => ("Nothing finished yet", "Tap the circle on a task to complete it."),
                _ => ("No tasks yet", "Tap + to add your first task.")
            };
        }

        [RelayCommand]
        private void SetFilter(string? value) => Filter = value ?? FilterAll;

        [RelayCommand]
        private async Task ToggleComplete(TodoItem? item)
        {
            if (item is null) return;
            item.IsCompleted = !item.IsCompleted;
            item.AddHistory(item.IsCompleted ? "Marked as completed" : "Marked as not done");
            await itemStore.UpdateItemAsync(item);
            ApplyFilter();
        }

        [RelayCommand]
        private Task NavigateToDetail(TodoItem? tappedItem)
        {
            if (tappedItem is null) return Task.CompletedTask;
            // Pass the ID of the tapped item to the detail page
            return NavigateAsync($"{nameof(ToDoDetailPage)}?{nameof(ToDoDetailViewModel.ItemId)}={tappedItem.IdToDoItem}");
        }

        [RelayCommand]
        private async Task NavigateToNew()
        {
            try
            {
                await NavigateAsync(nameof(NewToDoPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NavigateToNew navigation failed: {ex.Message}");
                await AlertAsync("Navigation error", "Unable to open the New Task page.");
            }
        }

        [RelayCommand]
        private Task GoBack() => GoBackAsync();
    }
}
