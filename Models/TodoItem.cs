namespace ToDoList.Models;

/// <summary>
/// A single to-do task / activity. Observable so the UI updates live when a
/// task is completed, pinned or edited.
/// </summary>
public partial class TodoItem : ObservableObject
{
    public const string PriorityHigh = "High";
    public const string PriorityMedium = "Medium";
    public const string PriorityLow = "Low";

    // Unique ID for each To-Do item
    public int IdToDoItem { get; set; }

    [ObservableProperty]
    private string itemName = string.Empty; // required

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasDescription))]
    private string? description = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOverdue), nameof(StatusText))]
    private bool isCompleted;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOverdue), nameof(DueText), nameof(StatusText), nameof(DateTimeText))]
    private DateTime? dueDate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TimeText), nameof(DateTimeText), nameof(IsOverdue))]
    private TimeSpan? startTime;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TimeText), nameof(DateTimeText), nameof(IsOverdue))]
    private TimeSpan? endTime;

    [ObservableProperty]
    private bool isPinned;

    /// <summary>Illustration file name, e.g. "fe_biking.png".</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Tint))]
    private string icon = TaskIcons.Default;

    [ObservableProperty]
    private string priority = PriorityMedium;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNotes))]
    private string notes = string.Empty;

    [ObservableProperty]
    private string category = "General";

    /// <summary>Things that happened to this task (created, completed, edited...).</summary>
    public List<ActivityEntry> History { get; set; } = new();

    public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    public bool HasNotes => !string.IsNullOrWhiteSpace(Notes);

    public DateTime? StartsAt => DueDate?.Date + (StartTime ?? TimeSpan.Zero);

    public DateTime? EndsAt
    {
        get
        {
            if (DueDate is null) return null;
            var end = DueDate.Value.Date + (EndTime ?? StartTime ?? new TimeSpan(23, 59, 0));
            // e.g. 11 PM - 7 AM ends the next day
            return StartsAt.HasValue && end < StartsAt.Value ? end.AddDays(1) : end;
        }
    }

    public bool IsOverdue => !IsCompleted && EndsAt.HasValue && EndsAt.Value < DateTime.Now;

    public string StatusText => IsCompleted ? "Completed" : IsOverdue ? "Overdue" : "In progress";

    /// <summary>Mockup style time range: "8.00-11.00 AM" or "11.00 PM-7.00 AM".</summary>
    public string TimeText
    {
        get
        {
            if (StartTime is null) return "All day";
            var s = DateTime.Today + StartTime.Value;
            if (EndTime is null) return s.ToString("h.mm tt");
            var e = DateTime.Today + EndTime.Value;
            return s.ToString("tt") == e.ToString("tt")
                ? $"{s:h.mm}-{e:h.mm tt}"
                : $"{s:h.mm tt}-{e:h.mm tt}";
        }
    }

    /// <summary>"Today", "Tomorrow", "Yesterday" or "Mon, Nov 9".</summary>
    public string DueText
    {
        get
        {
            if (DueDate is null) return "No date";
            var days = (DueDate.Value.Date - DateTime.Today).Days;
            return days switch
            {
                0 => "Today",
                1 => "Tomorrow",
                -1 => "Yesterday",
                _ => DueDate.Value.ToString("ddd, MMM d")
            };
        }
    }

    public string DateTimeText => $"{DueText} · {TimeText}";

    /// <summary>Pastel card color, picked from the illustration (matches the mockup palette).</summary>
    public Color Tint => TaskIcons.TintFor(Icon);

    public void AddHistory(string note) => History.Add(new ActivityEntry(DateTime.Now, note));
}

/// <summary>One line in a task's "Activity History".</summary>
public record ActivityEntry(DateTime When, string Note)
{
    public string DateText => When.ToString("dddd , dd MMMM yyyy");
    public string TimeText => When.ToString("hh.mm tt");
}

public static class TaskIcons
{
    public const string Default = "fe_notepad.png";

    /// <summary>Illustrations the user can pick for a task (Fluent Emoji, MIT).</summary>
    public static readonly string[] All =
    [
        "fe_biking.png", "fe_running.png", "fe_yoga.png", "fe_books.png", "fe_game.png",
        "fe_sleeping.png", "fe_cooking.png", "fe_cart.png", "fe_briefcase.png", "fe_laptop.png",
        "fe_email.png", "fe_phone.png", "fe_camera.png", "fe_airplane.png", "fe_popcorn.png",
        "fe_stethoscope.png", "fe_calendar.png", "fe_notepad.png"
    ];

    private static readonly Color Lavender = Color.FromArgb("#D8DCF2");
    private static readonly Color Lilac = Color.FromArgb("#E6DDF8");
    private static readonly Color Cream = Color.FromArgb("#F3EEE8");
    private static readonly Color Mist = Color.FromArgb("#E9EAEE");
    private static readonly Color Blush = Color.FromArgb("#FBEEF5");

    public static Color TintFor(string? icon) => icon switch
    {
        "fe_biking.png" or "fe_yoga.png" or "fe_stethoscope.png" or "fe_calendar.png" => Lavender,
        "fe_running.png" or "fe_airplane.png" or "fe_notepad.png" => Lilac,
        "fe_books.png" or "fe_cooking.png" or "fe_briefcase.png" => Cream,
        "fe_game.png" or "fe_laptop.png" or "fe_email.png" or "fe_phone.png" => Mist,
        _ => Blush // sleeping, cart, camera, popcorn
    };
}

public static class Categories
{
    public static readonly string[] All = ["General", "Work", "Personal", "Shopping", "Health"];
}
