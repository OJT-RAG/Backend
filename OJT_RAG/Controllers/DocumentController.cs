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

        // 🟢 CREATE - Upload Word
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

            string cloudinaryUrl = await _cloudinaryService.UploadFileAsync(file);
            string extractedText = await ExtractTextFromWord(file);

            JsonObject jsonContent = new JsonObject
            {
                ["ExtractedText"] = extractedText,
                ["WordCount"] = extractedText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length,
                ["FileName"] = file.FileName
            };

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
                JsonContent = jsonContent.ToJsonString()
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

        // 🟡 READ - Lấy tất cả tài liệu
        [HttpGet("all")]
        public IActionResult GetAllDocuments()
        {
            var documents = _context.Documents
                .OrderByDescending(d => d.UploadDate)
                .ToList();
            return Ok(documents);
        }

        // 🟡 READ - Lấy tài liệu theo user
        [HttpGet("user/{userId}")]
        public IActionResult GetDocumentsByUser(int userId)
        {
            var documents = _context.Documents
                .Where(d => d.UploadedBy == userId)
                .OrderByDescending(d => d.UploadDate)
                .ToList();

            if (!documents.Any())
                return NotFound("Người dùng này chưa upload tài liệu nào.");

            return Ok(documents);
        }

        // 🟡 READ - Lấy chi tiết 1 tài liệu theo ID
        [HttpGet("{id}")]
        public IActionResult GetDocumentById(int id)
        {
            var document = _context.Documents.Find(id);
            if (document == null)
                return NotFound($"Không tìm thấy tài liệu với ID = {id}");

            return Ok(document);
        }

        // 🟠 UPDATE - Cập nhật thông tin tài liệu (title, description, status, type, language)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] Document updatedDoc)
        {
            var document = _context.Documents.Find(id);
            if (document == null)
                return NotFound($"Không tìm thấy tài liệu với ID = {id}");

            document.Title = updatedDoc.Title ?? document.Title;
            document.Description = updatedDoc.Description ?? document.Description;
            document.Status = updatedDoc.Status;
            document.Type = updatedDoc.Type ?? document.Type;
            document.Language = updatedDoc.Language ?? document.Language;
            document.Version += 1; // tăng version mỗi lần update
            document.UploadDate = DateTime.UtcNow;

            _context.Documents.Update(document);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cập nhật tài liệu thành công ✅", Document = document });
        }

        // 🔴 DELETE - Xóa tài liệu
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = _context.Documents.Find(id);
            if (document == null)
                return NotFound($"Không tìm thấy tài liệu với ID = {id}");

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Xóa tài liệu thành công 🗑️", DocumentId = id });
        }

        // 📄 Hàm đọc text từ Word
        private async Task<string> ExtractTextFromWord(IFormFile file)
        {
            string tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            StringBuilder sb = new StringBuilder();
            using (var wordDoc = WordprocessingDocument.Open(tempPath, false))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                sb.Append(body.InnerText);
            }

            return sb.ToString();
        }
    }
}
