namespace OJT_RAG.DTOs.JobPositionDTO
{
    public class CreateJobPositionDTO
    {
        public long? MajorId { get; set; }

        public long? SemesterId { get; set; }

        // BẮT BUỘC - không nullable
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "SemesterCompanyId là bắt buộc")]
        public long SemesterCompanyId { get; set; }

        // BẮT BUỘC - không nullable
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "JobTitle là bắt buộc")]
        public string JobTitle { get; set; } = null!;

        public string? Requirements { get; set; }

        public string? Benefit { get; set; }

        public string? Location { get; set; }

        public string? SalaryRange { get; set; }

        // Default true nếu client không gửi
        public bool IsActive { get; set; } = true;
    }
}
