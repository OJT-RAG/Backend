namespace OJT_RAG.ModelView.JobPositionModelView
{
    public class JobPositionModelView
    {
        public long JobPositionId { get; set; }
        public long? MajorId { get; set; }
        public long? SemesterId { get; set; }
        public long SemesterCompanyId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string? Requirements { get; set; }
        public string? Benefit { get; set; }
        public string? Location { get; set; }
        public string? SalaryRange { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        // public string? CompanyName { get; set; }
    }
}