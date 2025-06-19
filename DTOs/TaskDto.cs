namespace TaskManagement.API.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string TaskType { get; set; }
        public bool IsClosed { get; set; }
        public int Status { get; set; }
        public DevelopmentDataDto? DevelopmentData { get; set; }
        public ProcurementDataDto? ProcurementData { get; set; }

    }

 
}
