namespace TaskManagement.API.DTOs
{
    public class CreateTaskDto
    {
        public string TaskType { get; set; } = string.Empty; // "Procurement" / "Development"
        public int AssignedUserId { get; set; }
    }
}
