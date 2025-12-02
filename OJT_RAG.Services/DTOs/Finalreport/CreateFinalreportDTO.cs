using Microsoft.AspNetCore.Http;

namespace OJT_RAG.DTOs.FinalreportDTO
{
    public class CreateFinalreportDTO
    {
        public long UserId { get; set; }
        public long JobPositionId { get; set; }
        public long SemesterId { get; set; }
        public string? StudentReportText { get; set; }

        public string? CompanyFeedback { get; set; }
        public int? CompanyRating { get; set; }
        public string? CompanyEvaluator { get; set; }

        // 🔥 File upload
        public IFormFile? File { get; set; }
    }
}
