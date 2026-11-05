namespace ToDoList.Models;

/// <summary>
/// A selectable chip in the UI (priority, category, illustration...).
/// Replaces the platform Picker, whose popup renders empty on Mac Catalyst.
/// </summary>
public partial class ChipOption : ObservableObject
{
    public ChipOption(string value, string icon = "", Color? accent = null)
    {
        Value = value;
        Icon = icon;
        Accent = accent ?? Colors.Transparent;
    }

    public string Value { get; }

    public string Icon { get; }

    public Color Accent { get; }

    [ObservableProperty]
    private bool isSelected;
}

/// <summary>A day in the home page date strip.</summary>
public partial class DayOption : ObservableObject
{
    public DayOption(DateTime date) => Date = date.Date;

    public DateTime Date { get; }

    public string DayName => Date.ToString("ddd").ToLowerInvariant();

    public string DayNumber => Date.ToString("dd");

    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private bool hasTasks;
}
