using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ToDoList.ViewModels;

public partial class NewToDoViewModel : BaseViewModel
{
    private readonly IitemStore<TodoItem> itemStore;

    public NewToDoViewModel(IitemStore<TodoItem> itemStore) : base("New Task")
    {
        this.itemStore = itemStore;
        DueDate = DateTime.Now.Date;
        Priority = "Medium";
    }

    [ObservableProperty]
    private string taskName = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private DateTime dueDate;

    [ObservableProperty]
    private string priority = "Medium";

    [ObservableProperty]
    private bool isCompleted;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private string category = "General";

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(TaskName))
        {
            var window = Application.Current?.Windows != null && Application.Current.Windows.Count > 0 ? Application.Current.Windows[0] : null;
            var page = window?.Page;
            if (page != null)
            {
                await page.DisplayAlertAsync("Validation", "Task name is required.", "OK");
            }
            else
            {
                Debug.WriteLine("Task name is required.");
            }
            return;
        }

        // determine a new id (mock store doesn't auto-assign)
        int newId = 1;
        try
        {
            var items = await itemStore.GetItemsAsync();
            if (items != null && items.Any())
            {
                newId = items.Max(i => i.IdToDoItem) + 1;
            }
        }
        catch
        {
            // ignore and use default id
        }

        var item = new TodoItem
        {
            IdToDoItem = newId,
            ItemName = TaskName,
            Description = Description,
            DueDate = DueDate,
            Priority = Priority,
            IsCompleted = IsCompleted,
            Notes = Notes,
            Category = Category
        };

        try
        {
            await itemStore.AddItemAsync(item);
            // Navigate back
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to save item: {ex.Message}");
            var window = Application.Current?.Windows != null && Application.Current.Windows.Count > 0 ? Application.Current.Windows[0] : null;
            var page = window?.Page;
            if (page != null)
            {
                await page.DisplayAlertAsync("Error", "Failed to save task.", "OK");
            }
            else
            {
                Debug.WriteLine("Failed to save task.");
            }
        }
    }

    [RelayCommand]
    private async Task Cancel()
    {
        try
        {
            await Shell.Current.GoToAsync("..", true);
        }
        catch
        {
            // ignore
        }
    }
}
