using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.SemesterCompanyDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/semester-company")]
    [ApiController]
    public class SemesterCompanyController : ControllerBase
    {
        private readonly ISemesterCompanyService _service;

        public SemesterCompanyController(ISemesterCompanyService service)
        {
            _service = service;
        }

        // ================= GET ALL =================
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _service.GetAll();
                return Ok(new { success = true, data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi lấy danh sách học kỳ - công ty.", error = ex.Message });
            }
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _service.GetById(id);
                if (data == null)
                    return NotFound(new { success = false, message = $"Không tìm thấy bản ghi với ID {id}" });

                return Ok(new { success = true, data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // ================= GET BY SEMESTER =================
        [HttpGet("semester/{semesterId}")]
        public async Task<IActionResult> GetBySemester(long semesterId)
        {
            try
            {
                var data = await _service.GetBySemester(semesterId);
                return Ok(new { success = true, data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // ================= GET BY COMPANY =================
        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(long companyId)
        {
            try
            {
                var data = await _service.GetByCompany(companyId);
                return Ok(new { success = true, data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // ================= CREATE =================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateSemesterCompanyDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.Create(dto);
                return Ok(new { success = true, message = "Tạo mới thành công", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi khi tạo liên kết.",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }

        }

        // ================= UPDATE =================
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateSemesterCompanyDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.Update(dto);
                if (result == null)
                    return NotFound(new { success = false, message = "Không tìm thấy dữ liệu để cập nhật." });

                return Ok(new { success = true, message = "Cập nhật thành công", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi cập nhật.", error = ex.Message });
            }
        }

        // ================= DELETE =================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var isDeleted = await _service.Delete(id);
                if (!isDeleted)
                    return NotFound(new { success = false, message = "Xóa thất bại: Không tìm thấy bản ghi." });

                return Ok(new { success = true, message = "Đã xóa thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi xóa bản ghi.", error = ex.Message });
            }
        }
    }
}