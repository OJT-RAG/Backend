using OJT_RAG.Models;

namespace OJT_RAG.Services
{
    public interface IDocumentService
    {
        /// <summary>
        /// Upload file (PDF hoặc DOCX) và lưu nội dung JSON vào database
        /// </summary>
        Task<Document> UploadDocumentAsync(IFormFile file, int uploadedBy);
    }
}
