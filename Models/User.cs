namespace TaskManagement.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        // משימות שהמשתמש אחראי עליהן
        public List<Task> AssignedTasks { get; set; } = new();
    }
}
