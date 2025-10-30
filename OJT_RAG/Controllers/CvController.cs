using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Data;
using OJT_RAG.Models;
using OJT_RAG.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public CvController(AppDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // 🟢 CREATE - Upload CV
        [HttpPost("upload")]
        public async Task<IActionResult> UploadCV(IFormFile file, int userId, string title)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ.");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Chỉ cho phép upload file PDF.");

            // Upload PDF lên Cloudinary
            string cloudinaryUrl = await _cloudinaryService.UploadPdfAsImageAsync(file);

            var cv = new Cv
            {
                UserId = userId,
                Title = title,
                FileName = file.FileName,
                LinkUrl = cloudinaryUrl,
                UploadDate = DateTime.UtcNow,
                Status = "Uploaded"
            };

            _context.Cvs.Add(cv);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tải lên CV thành công ✅",
                CvId = cv.CvId,
                Url = cloudinaryUrl
            });
        }

        // 🟡 READ - Lấy tất cả CV
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCvs()
        {
            var cvs = await _context.Cvs.OrderByDescending(c => c.UploadDate).ToListAsync();
            return Ok(cvs);
        }

        // 🟡 READ - Lấy CV theo người dùng
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCvsByUser(int userId)
        {
            var cvs = await _context.Cvs
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.UploadDate)
                .ToListAsync();

            if (!cvs.Any())
                return NotFound("Không tìm thấy CV nào cho người dùng này.");

            return Ok(cvs);
        }

        // 🟡 READ - Lấy chi tiết 1 CV
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCvById(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null)
                return NotFound($"Không tìm thấy CV có ID = {id}");

            return Ok(cv);
        }

        // 🟠 UPDATE - Cập nhật thông tin CV (title, status)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCv(int id, [FromBody] Cv updatedCv)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null)
                return NotFound($"Không tìm thấy CV có ID = {id}");

            cv.Title = updatedCv.Title ?? cv.Title;
            cv.Status = updatedCv.Status ?? cv.Status;
            cv.UploadDate = DateTime.UtcNow;

            _context.Cvs.Update(cv);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cập nhật CV thành công ✅", Cv = cv });
        }

        // 🔴 DELETE - Xóa CV
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCv(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null)
                return NotFound($"Không tìm thấy CV có ID = {id}");

            _context.Cvs.Remove(cv);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Xóa CV thành công 🗑️", CvId = id });
        }
    }
}
