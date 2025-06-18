namespace TaskManagement.API.Models
{
    public class DevelopmentData
    {
        public int TaskId { get; set; }
        public Task Task { get; set; } = null!;

        public string? Specification { get; set; }
        public string? BranchName { get; set; }
        public string? Version { get; set; }
    }
}
