namespace TaskManagement.API.Models
{
    public class ProcurementData
    {
        public int TaskId { get; set; }
        public Task Task { get; set; } = null!;

        public string? Offer1 { get; set; }
        public string? Offer2 { get; set; }
        public string? Receipt { get; set; }
    }
}
