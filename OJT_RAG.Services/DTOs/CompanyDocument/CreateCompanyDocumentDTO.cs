using Microsoft.AspNetCore.Http;

namespace OJT_RAG.DTOs.CompanyDocumentDTO
{
    public class CreateCompanyDocumentDTO
    {
        public long SemesterCompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public long? UploadedBy { get; set; }
        public bool? IsPublic { get; set; }

        public IFormFile? File { get; set; }
    }
}
