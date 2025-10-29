using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Data;
using OJT_RAG.Models;
using OJT_RAG.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCV(IFormFile file, int userId, string title)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ.");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Chỉ cho phép upload file PDF.");

            // Upload PDF → ảnh → Cloudinary
            string cloudinaryUrl = await _cloudinaryService.UploadPdfAsImageAsync(file);

            // Lưu CV vào DB
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
                Message = "Tải lên thành công ✅",
                CloudinaryUrl = cloudinaryUrl,
                CvId = cv.CvId
            });
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetCvsByUser(int userId)
        {
            var cvs = _context.Cvs.Where(c => c.UserId == userId).ToList();
            return Ok(cvs);
        }
    }
}
