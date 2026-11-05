namespace ToDoList.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class ToDoDetailViewModel : BaseViewModel
    {
        private readonly IitemStore<TodoItem> itemStore;

        public ToDoDetailViewModel(IitemStore<TodoItem> itemStore) : base("Your Activities")
        {
            this.itemStore = itemStore;
        }

        public ObservableCollection<ActivityEntry> History { get; } = new();

        [ObservableProperty]
        private string? itemId;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasItem))]
        private TodoItem? item;

        public bool HasItem => Item is not null;

        [ObservableProperty] private string countdownLabel = string.Empty;
        [ObservableProperty] private string countdownText = string.Empty;
        [ObservableProperty] private bool isHistoryExpanded = true;

        partial void OnItemIdChanged(string? value) => _ = LoadAsync();

        /// <summary>Reloads the task (called on navigation and every time the page appears, e.g. after editing).</summary>
        public async Task LoadAsync()
        {
            if (!int.TryParse(ItemId, out var id)) return;

            try
            {
                Item = await itemStore.GetItemAsync(id);
                RefreshHistory();
                Tick();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load item: {ex.Message}");
            }
        }

        private void RefreshHistory()
        {
            History.Clear();
            if (Item is null) return;
            foreach (var entry in Item.History.OrderByDescending(h => h.When))
            {
                History.Add(entry);
            }
        }

        /// <summary>Updates the "Starting in 00 : 05 : 05" card. Called every second by the page.</summary>
        public void Tick()
        {
            if (Item is null) return;

            var now = DateTime.Now;
            if (Item.IsCompleted)
            {
                CountdownLabel = "Completed";
                CountdownText = "Well done! ✓";
            }
            else if (Item.StartsAt is { } start && now < start)
            {
                CountdownLabel = "Starting in";
                CountdownText = Format(start - now);
            }
            else if (Item.EndsAt is { } end && now < end)
            {
                CountdownLabel = "Ends in";
                CountdownText = Format(end - now);
            }
            else
            {
                CountdownLabel = "Time's up";
                CountdownText = "Mark as done";
            }
        }

        private static string Format(TimeSpan span) => span.TotalDays >= 1
            ? $"{(int)span.TotalDays}d : {span.Hours:00}h : {span.Minutes:00}m"
            : $"{span.Hours:00} : {span.Minutes:00} : {span.Seconds:00}";

        [RelayCommand]
        private void ToggleHistory() => IsHistoryExpanded = !IsHistoryExpanded;

        [RelayCommand]
        private async Task ToggleComplete()
        {
            if (Item is null) return;
            Item.IsCompleted = !Item.IsCompleted;
            Item.AddHistory(Item.IsCompleted ? "Marked as completed" : "Marked as not done");
            await itemStore.UpdateItemAsync(Item);
            RefreshHistory();
            Tick();
        }

        [RelayCommand]
        private async Task More()
        {
            if (Item is null) return;

            const string edit = "Edit task", delete = "Delete task";
            var complete = Item.IsCompleted ? "Mark as not done" : "Mark as completed";
            var pin = Item.IsPinned ? "Unpin" : "Pin to home";

            var choice = await Shell.Current.DisplayActionSheetAsync(Item.ItemName, "Cancel", delete, edit, complete, pin);

            if (choice == edit)
            {
                await NavigateAsync($"{nameof(NewToDoPage)}?{nameof(NewToDoViewModel.ItemId)}={Item.IdToDoItem}");
            }
            else if (choice == complete)
            {
                await ToggleComplete();
            }
            else if (choice == pin)
            {
                Item.IsPinned = !Item.IsPinned;
                Item.AddHistory(Item.IsPinned ? "Pinned to home" : "Unpinned");
                await itemStore.UpdateItemAsync(Item);
                RefreshHistory();
            }
            else if (choice == delete)
            {
                var confirmed = await ConfirmAsync("Delete task", $"Delete “{Item.ItemName}”? This can't be undone.", "Delete");
                if (!confirmed) return;

                await itemStore.DeleteItemAsync(Item.IdToDoItem);
                await GoBackAsync();
            }
        }

        [RelayCommand]
        private Task GoBack() => GoBackAsync();
    }
}
