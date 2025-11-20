using Microsoft.AspNetCore.Http;

namespace OJT_RAG.DTOs.OjtDocumentDTO
{
    public class CreateOjtDocumentDTO
    {
        public string? Title { get; set; }
        public long? SemesterId { get; set; }
        public bool? IsGeneral { get; set; }
        public long? UploadedBy { get; set; }
        public IFormFile? File { get; set; }
    }
}
