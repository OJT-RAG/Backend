using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OJT_RAG.Data;
using OJT_RAG.Models;
using OJT_RAG.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using DocumentFormat.OpenXml.Packaging;
using WordDocument = DocumentFormat.OpenXml.Wordprocessing.Document;
using WordprocessingDocument = DocumentFormat.OpenXml.Packaging.WordprocessingDocument;

using System.Text;

namespace OJT_RAG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public DocumentController(AppDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        /// <summary>
        /// Upload file Word (.docx), lưu lên Cloudinary và database
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(
            IFormFile file,
            int uploadedBy,
            string title,
            string? description = null,
            string? language = "Vietnamese",
            string? type = "Policy")
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".docx")
                return BadRequest("Chỉ cho phép upload file .docx (Word 2007 trở lên).");

            // Upload file lên Cloudinary
            string cloudinaryUrl = await _cloudinaryService.UploadFileAsync(file);

            // Đọc nội dung file Word (.docx)
            string extractedText = await ExtractTextFromWord(file);
            JsonObject jsonContent = new JsonObject
            {
                ["ExtractedText"] = extractedText,
                ["WordCount"] = extractedText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length,
                ["FileName"] = file.FileName
            };

            // Tạo đối tượng Document
            var document = new Document
            {
                Title = title,
                Description = description,
                FilePath = cloudinaryUrl,
                UploadDate = DateTime.UtcNow,
                UploadedBy = uploadedBy,
                Language = language ?? "Vietnamese",
                Type = type ?? "Policy",
                Version = 1,
               Status = DocumentStatus.Draft,
                JsonContent = jsonContent?.ToJsonString()
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tải lên tài liệu thành công ✅",
                CloudinaryUrl = cloudinaryUrl,
                DocumentId = document.DocumentId
            });
        }

        /// <summary>
        /// Đọc text từ file .docx bằng OpenXML (miễn phí, không cần license)
        /// </summary>
        private async Task<string> ExtractTextFromWord(IFormFile file)
        {
            string tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            StringBuilder sb = new StringBuilder();

            // Dùng WordprocessingDocument (đúng chuẩn OpenXML)
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(tempPath, false))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                sb.Append(body.InnerText);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Lấy danh sách tài liệu theo người upload
        /// </summary>
        [HttpGet("user/{userId}")]
        public IActionResult GetDocumentsByUser(int userId)
        {
            var documents = _context.Documents
                .Where(d => d.UploadedBy == userId)
                .OrderByDescending(d => d.UploadDate)
                .ToList();

            return Ok(documents);
        }
    }
}
