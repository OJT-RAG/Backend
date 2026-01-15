namespace OJT_RAG.DTOs.JobPositionDTO
{
    public class UpdateJobPositionDTO
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "JobPositionId là bắt buộc")]
        public long JobPositionId { get; set; }

        public long? MajorId { get; set; }

        public long? SemesterId { get; set; }

        public long? SemesterCompanyId { get; set; } // Optional khi update

        public string? JobTitle { get; set; }

        public string? Requirements { get; set; }

        public string? Benefit { get; set; }

        public string? Location { get; set; }

        public string? SalaryRange { get; set; }

        public bool? IsActive { get; set; }
    }
}
