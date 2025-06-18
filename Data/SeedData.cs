using TaskManagement.API.Models;

namespace TaskManagement.API.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users.Any())
                return; // Seed כבר בוצע

            // משתמשים לדוגמה
            var users = new List<User>
            {
                new User { FullName = "Alice Admin" },
                new User { FullName = "Bob Builder" },
                new User { FullName = "Charlie Coder" }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // משימת Procurement ראשונית
            var task = new Models.Task
            {
                TaskType = "Procurement",
                Status = 1,
                IsClosed = false,
                AssignedUserId = users[0].Id
            };

            context.Tasks.Add(task);
            context.SaveChanges();

            // יצירת אובייקט ProcurementData מקושר למשימה
            var procurementData = new ProcurementData
            {
                TaskId = task.Id,
                Offer1 = "1000₪",
                Offer2 = "1200₪",
                Receipt = ""
            };

            context.ProcurementData.Add(procurementData);
            context.SaveChanges();
        }
    }
}
