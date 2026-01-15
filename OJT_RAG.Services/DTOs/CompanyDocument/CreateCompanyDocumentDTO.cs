using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
namespace OJT_RAG.DTOs.CompanyDocumentDTO
{
    public class CreateCompanyDocumentDTO
    {
        public long SemesterCompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        [JsonIgnore] // Ẩn khỏi Swagger và kết quả trả về
        public long? UploadedBy { get; set; }
        public bool? IsPublic { get; set; }

        public IFormFile? File { get; set; }
    }
}
