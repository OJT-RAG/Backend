using Microsoft.AspNetCore.Http;

namespace OJT_RAG.DTOs.OjtDocumentDTO
{
    public class UpdateOjtDocumentDTO
    {
        public long OjtdocumentId { get; set; }
        public string? Title { get; set; }
        public long? SemesterId { get; set; }
        public bool? IsGeneral { get; set; }
        public IFormFile? File { get; set; }
    }
}
