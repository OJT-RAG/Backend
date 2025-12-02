namespace OJT_RAG.Model.JobPositionModel
{
    public class CreateJobPositionModel
    {
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
