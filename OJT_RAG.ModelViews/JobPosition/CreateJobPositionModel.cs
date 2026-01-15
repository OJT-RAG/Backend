namespace OJT_RAG.Model.JobPositionModel
{
    // Model nội bộ dùng trong service (nếu cần mapping từ DTO)
    // Giữ nhất quán với DTO trên
    public class CreateJobPositionModel
    {
        public long? MajorId { get; set; }
        public long? SemesterId { get; set; }

        // Bắt buộc
        public long SemesterCompanyId { get; set; }

        public string JobTitle { get; set; } = null!;

        public string? Requirements { get; set; }
        public string? Benefit { get; set; }
        public string? Location { get; set; }
        public string? SalaryRange { get; set; }

        public bool IsActive { get; set; } = true;
    }
}