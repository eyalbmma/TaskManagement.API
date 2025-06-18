using TaskManagement.API.Models;
namespace TaskManagement.API.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string TaskType { get; set; } = string.Empty;
        public int Status { get; set; } = 1;
        public bool IsClosed { get; set; } = false;

        public int AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public ProcurementData? ProcurementData { get; set; }
        public DevelopmentData? DevelopmentData { get; set; }
    }
}