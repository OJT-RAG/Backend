using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OJT_RAG.ModelViews.CompanyDocument
{
    public class CreateCompanyDocumentModel
    {
        [Required]
        public long SemesterCompanyId { get; set; }

        [Required]
        public IFormFile FileUpload { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        public string? Description { get; set; }
    }
}