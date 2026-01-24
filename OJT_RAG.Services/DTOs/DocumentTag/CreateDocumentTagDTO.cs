using OJT_RAG.Repositories.Enums;

namespace OJT_RAG.DTOs.DocumentTagDTO
{
    public class CreateDocumentTagDTO
    {
        public string? Name { get; set; }
        public DocumentTagType Type { get; set; }
    }
}
