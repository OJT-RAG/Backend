using OJT_RAG.Repositories.Enums;

namespace OJT_RAG.DTOs.DocumentTagDTO
{
    public class UpdateDocumentTagDTO
    {
        public long DocumenttagId { get; set; }
        public string? Name { get; set; }
        public DocumentTagType? Type { get; set; }
    }
}
