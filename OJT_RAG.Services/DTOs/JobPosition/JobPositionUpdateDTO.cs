namespace OJT_RAG.DTOs.JobPositionDTO
{
    public class UpdateJobPositionDTO
    {
        public long JobPositionId { get; set; }
        public long? MajorId { get; set; }
        public long? SemesterId { get; set; }
        public string? JobTitle { get; set; }
        public string? Requirements { get; set; }
        public string? Benefit { get; set; }
        public string? Location { get; set; }
        public string? SalaryRange { get; set; }
        public bool? IsActive { get; set; }
    }
}
