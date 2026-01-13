using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OJT_RAG.DTOs.JobPositionDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/job-position")]
    public class JobPositionController : ControllerBase
    {
        private readonly IJobPositionService _service;

        public JobPositionController(IJobPositionService service)
        {
            _service = service;
        }

        // ================= GET ALL =================
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _service.GetAll();
                return Ok(new { success = true, data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi lấy danh sách vị trí công việc.", error = ex.Message });
            }
        }

        // ================= GET BY ID =================
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _service.GetById(id);
                if (data == null)
                    return NotFound(new { success = false, message = $"Không tìm thấy vị trí công việc với ID {id}" });

                return Ok(new { success = true, data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // ================= CREATE =================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateJobPositionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Dữ liệu không hợp lệ",
                    errors = ModelState
                });

            try
            {
                var result = await _service.Create(dto);

                return Ok(new
                {
                    success = true,
                    message = "Tạo vị trí công việc thành công",
                    data = result
                });
            }
            catch (DbUpdateException ex)
            {
                // Lỗi DB thật (FK, length, not null…)
                return BadRequest(new
                {
                    success = false,
                    message = "Lỗi dữ liệu khi lưu vào database",
                    error = ex.InnerException?.Message
                });
            }
            catch (ArgumentException ex)
            {
                // Lỗi nghiệp vụ (vd: major/semester không tồn tại)
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Lỗi không xác định
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi hệ thống",
                    error = ex.Message
                });
            }
        }


        // ================= UPDATE =================
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateJobPositionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, errors = ModelState });

            try
            {
                var result = await _service.Update(dto);
                if (result == null)
                    return NotFound(new { success = false, message = "Không tìm thấy vị trí công việc để cập nhật." });

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
                // Kiểm tra tồn tại trước khi xóa
                var exists = await _service.GetById(id);
                if (exists == null)
                    return NotFound(new { success = false, message = "Không tìm thấy vị trí công việc để xóa." });

                var result = await _service.Delete(id);
                return Ok(new { success = true, message = "Xóa vị trí công việc thành công.", data = result });
            }
            catch (Exception ex)
            {
                // Bắt lỗi nếu vị trí này đang được sử dụng bởi các bảng khác (Foreign Key Constraint)
                if (ex.InnerException?.Message.Contains("REFERENCE constraint") == true)
                    return BadRequest(new { success = false, message = "Không thể xóa vì vị trí này đang có dữ liệu liên quan." });

                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}