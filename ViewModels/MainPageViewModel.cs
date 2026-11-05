namespace ToDoList.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        public const string UserNameKey = "UserName";

        private readonly IitemStore<TodoItem> itemStore;
        private List<TodoItem> allItems = new();

        public MainPageViewModel(IitemStore<TodoItem> itemStore) : base("Home")
        {
            this.itemStore = itemStore;
            BuildDays();
        }

        public ObservableCollection<TodoItem> PinnedTasks { get; } = new();

        public ObservableCollection<DayOption> Days { get; } = new();

        public ObservableCollection<TodoItem> DayTasks { get; } = new();

        [ObservableProperty] private string userName = Preferences.Default.Get(UserNameKey, "Buddy");
        [ObservableProperty] private string subtitle = string.Empty;
        [ObservableProperty] private DateTime selectedDate = DateTime.Today;
        [ObservableProperty] private string dayTitle = "All Task";
        [ObservableProperty] private bool isPinnedExpanded = true;
        [ObservableProperty] private bool hasPinned;
        [ObservableProperty] private bool hasDayTasks;

        /// <summary>Two weeks starting 2 days ago, so you can peek back and plan ahead.</summary>
        private void BuildDays()
        {
            Days.Clear();
            for (var i = -2; i < 12; i++)
            {
                Days.Add(new DayOption(DateTime.Today.AddDays(i)) { IsSelected = i == 0 });
            }
        }

        public async Task RefreshAsync()
        {
            if (Days.Count == 0 || Days[2].Date != DateTime.Today)
            {
                // the app was left open past midnight
                BuildDays();
                SelectedDate = DateTime.Today;
            }

            UserName = Preferences.Default.Get(UserNameKey, "Buddy");
            allItems = (await itemStore.GetItemsAsync()).ToList();

            var todayCount = allItems.Count(i => i.DueDate?.Date == DateTime.Today && !i.IsCompleted);
            Subtitle = todayCount switch
            {
                0 => "You're all done for today",
                1 => "Today you have 1 task",
                _ => $"Today you have {todayCount} tasks"
            };

            PinnedTasks.Clear();
            foreach (var item in allItems.Where(i => i.IsPinned).OrderBy(i => i.IsCompleted).ThenBy(i => i.StartsAt))
            {
                PinnedTasks.Add(item);
            }
            HasPinned = PinnedTasks.Count > 0;

            foreach (var day in Days)
            {
                day.HasTasks = allItems.Any(i => i.DueDate?.Date == day.Date);
            }

            ApplyDay();
        }

        private void ApplyDay()
        {
            foreach (var day in Days) day.IsSelected = day.Date == SelectedDate;

            DayTitle = SelectedDate == DateTime.Today ? "All Task" : SelectedDate.ToString("dddd, MMM d");

            DayTasks.Clear();
            foreach (var item in allItems
                         .Where(i => i.DueDate?.Date == SelectedDate)
                         .OrderBy(i => i.IsCompleted)
                         .ThenBy(i => i.StartTime))
            {
                DayTasks.Add(item);
            }
            HasDayTasks = DayTasks.Count > 0;
        }

        [RelayCommand]
        private void SelectDay(DayOption? day)
        {
            if (day is null) return;
            SelectedDate = day.Date;
            ApplyDay();
        }

        [RelayCommand]
        private void TogglePinned() => IsPinnedExpanded = !IsPinnedExpanded;

        [RelayCommand]
        private Task OpenTask(TodoItem? item) => item is null
            ? Task.CompletedTask
            : NavigateAsync($"{nameof(ToDoDetailPage)}?{nameof(ToDoDetailViewModel.ItemId)}={item.IdToDoItem}");

        [RelayCommand]
        private Task AddTask() =>
            NavigateAsync($"{nameof(NewToDoPage)}?{nameof(NewToDoViewModel.Date)}={SelectedDate:yyyy-MM-dd}");

        [RelayCommand]
        private Task OpenAllTasks() => NavigateAsync(nameof(ToDoItemPage));

        [RelayCommand]
        private async Task Menu()
        {
            const string all = "All tasks", add = "New task", name = "Change my name", intro = "Show welcome screen";
            var choice = await Shell.Current.DisplayActionSheetAsync("Menu", "Close", null, all, add, name, intro);

            switch (choice)
            {
                case all:
                    await OpenAllTasks();
                    break;
                case add:
                    await AddTask();
                    break;
                case name:
                    var result = await Shell.Current.DisplayPromptAsync("Your name", "What should we call you?",
                        "Save", "Cancel", initialValue: UserName, maxLength: 20);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        Preferences.Default.Set(UserNameKey, result.Trim());
                        UserName = result.Trim();
                    }
                    break;
                case intro:
                    await Shell.Current.GoToAsync("//Onboarding", true);
                    break;
            }
        }
    }
}
