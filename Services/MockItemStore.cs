
﻿
namespace ToDoList.Services
{
    public class MockItemStore : IitemStore<TodoItem>
    {
        readonly ObservableCollection<TodoItem> LsToDoItem;

        public MockItemStore()
        {
            LsToDoItem = new ObservableCollection<TodoItem>()
            {
                new TodoItem { IdToDoItem = 1, ItemName = "Go to the doctor", Description = "It's near Annecy center to get an appointment", IsCompleted = true, DueDate = DateTime.Now.AddDays(2), Priority = "High", Notes = "Bring medical records" },
                new TodoItem { IdToDoItem = 2, ItemName = "Watch Netflix", Description = "Discover new series to enjoy", IsCompleted = true, DueDate = DateTime.Now.AddDays(1), Priority = "Low" },
                new TodoItem { IdToDoItem = 3, ItemName = "Post on Instagram", Description = "Create engaging content using Canva daily", IsCompleted = false, DueDate = DateTime.Now.AddDays(3), Priority = "Medium" },
                new TodoItem { IdToDoItem = 4, ItemName = "Buy Groceries", Description = "Get essentials from the supermarket", IsCompleted = true, DueDate = DateTime.Now.AddDays(1), Priority = "High", Notes = "Check for discounts" },
                new TodoItem { IdToDoItem = 5, ItemName = "Read Something", Description = "Develop a daily reading habit", IsCompleted = false, DueDate = DateTime.Now.AddDays(7), Priority = "Medium", Notes = "Focus on self-help books" },
                new TodoItem { IdToDoItem = 6, ItemName = "Reply to Emails", Description = "Respond to pending emails from friends and colleagues", IsCompleted = true, DueDate = DateTime.Now.AddDays(1), Priority = "High" },
                new TodoItem { IdToDoItem = 7, ItemName = "Plan a trip", Description = "Organize a weekend getaway to Annecy", IsCompleted = false, DueDate = DateTime.Now.AddDays(10), Priority = "Medium", Notes = "Book a hotel" },
                new TodoItem { IdToDoItem = 8, ItemName = "Workout", Description = "Daily morning yoga and exercise", IsCompleted = false, DueDate = DateTime.Now, Priority = "High", Notes = "Focus on flexibility exercises" },
                new TodoItem { IdToDoItem = 9, ItemName = "Call Mom", Description = "Catch up on the latest family news", IsCompleted = false, DueDate = DateTime.Now.AddDays(1), Priority = "High" },
                new TodoItem { IdToDoItem = 10, ItemName = "Cook Dinner", Description = "Prepare a healthy dinner with veggies", IsCompleted = true, DueDate = DateTime.Now, Priority = "Medium", Notes = "Try a new recipe" }
            };
        }

        public async Task<bool> AddItemAsync(TodoItem ToDoItem)
        {
            LsToDoItem.Add(ToDoItem);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(TodoItem ToDoItem)
        {
            var OldToDoItem = LsToDoItem.Where((TodoItem arg) => arg.IdToDoItem == ToDoItem.IdToDoItem).FirstOrDefault();
            LsToDoItem.Remove(OldToDoItem);
            LsToDoItem.Add(ToDoItem);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var OldToDoItem = LsToDoItem.Where((TodoItem arg) => arg.IdToDoItem == id).FirstOrDefault();
            LsToDoItem.Remove(OldToDoItem);

            return await Task.FromResult(true);
        }

        public async Task<TodoItem> GetItemAsync(int id)
        {
            return await Task.FromResult(LsToDoItem.FirstOrDefault(s => s.IdToDoItem == id));
        }

        public async Task<IEnumerable<TodoItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(LsToDoItem);
        }
    }
}
