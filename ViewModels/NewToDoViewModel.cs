namespace ToDoList.ViewModels;

/// <summary>
/// Backs the Create / Edit task page.
/// ?ItemId=n edits an existing task, ?Date=yyyy-MM-dd pre-selects a day for a new one.
/// </summary>
[QueryProperty(nameof(ItemId), nameof(ItemId))]
[QueryProperty(nameof(Date), nameof(Date))]
public partial class NewToDoViewModel : BaseViewModel
{
    private readonly IitemStore<TodoItem> itemStore;
    private TodoItem? editing;

    public NewToDoViewModel(IitemStore<TodoItem> itemStore) : base("New Task")
    {
        this.itemStore = itemStore;

        PriorityOptions =
        [
            new ChipOption(TodoItem.PriorityLow, accent: Color.FromArgb("#4CAF7A")),
            new ChipOption(TodoItem.PriorityMedium, accent: Color.FromArgb("#E9A23B")),
            new ChipOption(TodoItem.PriorityHigh, accent: Color.FromArgb("#E05A5A"))
        ];

        CategoryOptions = new(Categories.All.Select(c => new ChipOption(c)));

        IconOptions = new(TaskIcons.All.Select(i => new ChipOption(i, i, TaskIcons.TintFor(i))));

        // Default slot: next full hour, one hour long.
        var next = DateTime.Now.AddHours(1);
        StartTime = new TimeSpan(next.Hour, 0, 0);
        EndTime = new TimeSpan((next.Hour + 1) % 24, 0, 0);

        SyncChips();
    }

    public ObservableCollection<ChipOption> PriorityOptions { get; }

    public ObservableCollection<ChipOption> CategoryOptions { get; }

    public ObservableCollection<ChipOption> IconOptions { get; }

    [ObservableProperty] private string? itemId;
    [ObservableProperty] private string? date;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SaveButtonText))]
    private bool isEditMode;

    public string SaveButtonText => IsEditMode ? "Save Changes" : "Create Task";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNameError))]
    private string nameError = string.Empty;

    public bool HasNameError => !string.IsNullOrEmpty(NameError);

    [ObservableProperty] private string taskName = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private DateTime dueDate = DateTime.Today;
    [ObservableProperty] private TimeSpan startTime;
    [ObservableProperty] private TimeSpan endTime;
    [ObservableProperty] private string priority = TodoItem.PriorityMedium;
    [ObservableProperty] private string category = "General";
    [ObservableProperty] private string icon = TaskIcons.Default;
    [ObservableProperty] private bool isPinned;
    [ObservableProperty] private bool isCompleted;
    [ObservableProperty] private string notes = string.Empty;

    partial void OnTaskNameChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value)) NameError = string.Empty;
    }

    partial void OnPriorityChanged(string value) => SyncChips();

    partial void OnCategoryChanged(string value) => SyncChips();

    partial void OnIconChanged(string value) => SyncChips();

    partial void OnDateChanged(string? value)
    {
        if (!IsEditMode && DateTime.TryParse(value, out var d)) DueDate = d.Date;
    }

    partial void OnItemIdChanged(string? value) => _ = LoadForEditAsync(value);

    private async Task LoadForEditAsync(string? value)
    {
        if (!int.TryParse(value, out var id)) return;

        var item = await itemStore.GetItemAsync(id);
        if (item is null) return;

        editing = item;
        IsEditMode = true;
        Title = "Edit Task";
        TaskName = item.ItemName;
        Description = item.Description ?? string.Empty;
        DueDate = item.DueDate ?? DateTime.Today;
        StartTime = item.StartTime ?? StartTime;
        EndTime = item.EndTime ?? EndTime;
        Priority = item.Priority;
        Category = item.Category;
        Icon = item.Icon;
        IsPinned = item.IsPinned;
        Notes = item.Notes;
        IsCompleted = item.IsCompleted;
    }

    private void SyncChips()
    {
        foreach (var chip in PriorityOptions) chip.IsSelected = chip.Value == Priority;
        foreach (var chip in CategoryOptions) chip.IsSelected = chip.Value == Category;
        foreach (var chip in IconOptions) chip.IsSelected = chip.Value == Icon;
    }

    [RelayCommand]
    private void SelectPriority(ChipOption? option)
    {
        if (option is not null) Priority = option.Value;
    }

    [RelayCommand]
    private void SelectCategory(ChipOption? option)
    {
        if (option is not null) Category = option.Value;
    }

    [RelayCommand]
    private void SelectIcon(ChipOption? option)
    {
        if (option is not null) Icon = option.Value;
    }

    [RelayCommand]
    private void TogglePinned() => IsPinned = !IsPinned;

    /// <summary>Quick date shortcuts: parameter is the number of days from today.</summary>
    [RelayCommand]
    private void QuickDate(string? daysFromToday)
    {
        if (int.TryParse(daysFromToday, out var days))
        {
            DueDate = DateTime.Today.AddDays(days);
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(TaskName))
        {
            NameError = "Please give your task a name.";
            return;
        }

        var item = new TodoItem
        {
            IdToDoItem = editing?.IdToDoItem ?? 0, // 0 => store assigns a new id
            ItemName = TaskName.Trim(),
            Description = Description?.Trim(),
            DueDate = DueDate,
            StartTime = StartTime,
            EndTime = EndTime,
            Priority = Priority,
            Category = Category,
            Icon = Icon,
            IsPinned = IsPinned,
            IsCompleted = IsCompleted,
            Notes = Notes?.Trim() ?? string.Empty,
            History = editing?.History.ToList() ?? new()
        };
        item.AddHistory(IsEditMode ? "Task updated" : "Task created");

        try
        {
            IsBusy = true;
            if (IsEditMode)
                await itemStore.UpdateItemAsync(item);
            else
                await itemStore.AddItemAsync(item);

            await GoBackAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to save item: {ex.Message}");
            await AlertAsync("Error", "Failed to save task.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task Cancel()
    {
        try
        {
            await GoBackAsync();
        }
        catch
        {
            // ignore
        }
    }
}
