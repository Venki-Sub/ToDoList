namespace ToDoList.Models;

public class TodoItem
{
// Unique ID for each To-Do item
        public int IdToDoItem { get; set; } 

        // The Item name
        public string ItemName { get; set; }
        public string Description { get; set; }

        // Whether the task is completed
        public bool IsCompleted { get; set; }

        // Optional due date
        public DateTime? DueDate { get; set; }

        // Priority of the task (Optional)
        public string Priority { get; set; } // e.g., "High", "Medium", "Low"

        // Additional notes (Optional)
        public string Notes { get; set; }

        // Category (Optional)
        public string Category { get; set; }

        public TodoItem()
        {
            ItemName = string.Empty;
            Description = string.Empty;
            Priority = "Medium";
            Notes = string.Empty;
            Category = "General";
        }
}