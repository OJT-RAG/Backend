using Microsoft.AspNetCore.Http;

namespace OJT_RAG.ModelViews.CompanyDocument
{
    public class UpdateCompanyDocumentModel
    {
        public string? FileName { get; set; }
        public string? Description { get; set; }

        // Cho phép update file mới
        public IFormFile? NewFile { get; set; }
    }
}
