namespace ToDoList.Services
{
    /// <summary>In-memory store (data resets when the app restarts).</summary>
    public class MockItemStore : IitemStore<TodoItem>
    {
        private readonly List<TodoItem> items;

        public MockItemStore()
        {
            var today = DateTime.Today;
            static TimeSpan T(int h, int m = 0) => new(h, m, 0);

            items =
            [
                Seed(1, "Cycling Around", "Morning ride around the lake to start the day fresh.", today, T(8), T(11), "fe_biking.png", "Health", TodoItem.PriorityHigh, pinned: true, notes: "Check the tyre pressure first"),
                Seed(2, "Evening Run", "5 km easy run in the park.", today, T(16), T(17), "fe_running.png", "Health", TodoItem.PriorityMedium, pinned: true),
                Seed(3, "Reading Book", "Develop a daily reading habit — 30 pages tonight.", today, T(20), T(23), "fe_books.png", "Personal", TodoItem.PriorityMedium, notes: "Focus on self-help books"),
                Seed(4, "Playing Game", "Weekly online session with friends.", today, T(12), T(13), "fe_game.png", "Personal", TodoItem.PriorityLow),
                Seed(5, "Sleeping Time", "Phone away, lights off, 8 hours of rest.", today, T(23), T(7), "fe_sleeping.png", "Personal", TodoItem.PriorityHigh),
                Seed(6, "Go to the doctor", "Annual check-up near the city center.", today.AddDays(1), T(10), T(11), "fe_stethoscope.png", "Health", TodoItem.PriorityHigh, notes: "Bring medical records"),
                Seed(7, "Buy Groceries", "Get essentials from the supermarket.", today.AddDays(1), T(18), T(19), "fe_cart.png", "Shopping", TodoItem.PriorityMedium, notes: "Check for discounts"),
                Seed(8, "Reply to Emails", "Respond to pending emails from colleagues.", today.AddDays(-1), T(9), T(10), "fe_email.png", "Work", TodoItem.PriorityHigh, completed: true),
                Seed(9, "Call Mom", "Catch up on the latest family news.", today.AddDays(-1), T(19), T(20), "fe_phone.png", "Personal", TodoItem.PriorityHigh, completed: true),
                Seed(10, "Morning Yoga", "Stretching and breathing exercises.", today.AddDays(2), T(7), T(8), "fe_yoga.png", "Health", TodoItem.PriorityMedium),
                Seed(11, "Plan a trip", "Organize a weekend getaway.", today.AddDays(3), T(15), T(16), "fe_airplane.png", "Personal", TodoItem.PriorityLow, notes: "Book a hotel"),
                Seed(12, "Cook Dinner", "Prepare a healthy dinner with veggies.", today.AddDays(2), T(19), T(20), "fe_cooking.png", "Personal", TodoItem.PriorityMedium, notes: "Try a new recipe")
            ];
        }

        private static TodoItem Seed(int id, string name, string description, DateTime date, TimeSpan start, TimeSpan end,
            string icon, string category, string priority, bool pinned = false, bool completed = false, string notes = "")
        {
            var item = new TodoItem
            {
                IdToDoItem = id,
                ItemName = name,
                Description = description,
                DueDate = date,
                StartTime = start,
                EndTime = end,
                Icon = icon,
                Category = category,
                Priority = priority,
                IsPinned = pinned,
                IsCompleted = completed,
                Notes = notes
            };

            // A bit of history so the "Activity History" section has content.
            var t = date.Date + start;
            item.History.Add(new ActivityEntry(t.AddDays(-3), "Task created"));
            item.History.Add(new ActivityEntry(t.AddDays(-2).AddMinutes(30), "One time, yeah !"));
            item.History.Add(new ActivityEntry(t.AddDays(-1), "Great time."));
            if (completed) item.History.Add(new ActivityEntry(t.AddMinutes(45), "Marked as completed"));
            return item;
        }

        public Task<bool> AddItemAsync(TodoItem item)
        {
            if (item.IdToDoItem <= 0)
            {
                item.IdToDoItem = items.Count == 0 ? 1 : items.Max(i => i.IdToDoItem) + 1;
            }

            items.Add(item);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateItemAsync(TodoItem item)
        {
            var index = items.FindIndex(i => i.IdToDoItem == item.IdToDoItem);
            if (index < 0) return Task.FromResult(false);

            // Replace in place so the item keeps its position.
            items[index] = item;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            var removed = items.RemoveAll(i => i.IdToDoItem == id) > 0;
            return Task.FromResult(removed);
        }

        public Task<TodoItem?> GetItemAsync(int id)
        {
            return Task.FromResult(items.FirstOrDefault(i => i.IdToDoItem == id));
        }

        public Task<IEnumerable<TodoItem>> GetItemsAsync(bool forceRefresh = false)
        {
            // Return a snapshot so callers can't mutate the store's list.
            return Task.FromResult<IEnumerable<TodoItem>>(items.ToList());
        }
    }
}
