using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OJT_RAG.DTOs.JobPositionDTO;
using OJT_RAG.ModelView.JobPositionModelView;
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

        // GET ALL
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
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi khi lấy danh sách vị trí công việc.",
                    error = ex.Message
                });
            }
        }

        // GET BY ID
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _service.GetById(id);
                if (data == null)
                    return NotFound(new
                    {
                        success = false,
                        message = $"Không tìm thấy vị trí công việc với ID {id}"
                    });

                return Ok(new { success = true, data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi hệ thống.",
                    error = ex.Message
                });
            }
        }

        // CREATE
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateJobPositionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Dữ liệu không hợp lệ",
                    errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            try
            {
                await _service.Create(dto);
                return Ok(new
                {
                    success = true,
                    message = "Tạo vị trí công việc thành công"
                });
            }
            catch (ArgumentException ex)
            {
                // Lỗi từ service (ví dụ: SemesterCompanyId không tồn tại)
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                // Lỗi DB (FK violation, not null, etc.)
                return BadRequest(new
                {
                    success = false,
                    message = "Lỗi dữ liệu khi lưu vào database (kiểm tra semester_company_id hợp lệ chưa)",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi hệ thống khi tạo",
                    error = ex.Message
                });
            }
        }

        // UPDATE
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateJobPositionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });

            try
            {
                var success = await _service.Update(dto);
                if (!success)
                    return NotFound(new
                    {
                        success = false,
                        message = "Không tìm thấy vị trí công việc để cập nhật."
                    });

                return Ok(new { success = true, message = "Cập nhật vị trí công việc thành công" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi khi cập nhật.",
                    error = ex.Message
                });
            }
        }

        // DELETE
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            // 1️⃣ Check tồn tại
            var exists = await _service.GetById(id);
            if (exists == null)
                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy vị trí công việc để xóa."
                });

            // 2️⃣ Check nghiệp vụ: có JobApplication không?
            var hasApplication = await _service.HasJobApplication(id);
            if (hasApplication)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Không thể xóa vị trí công việc vì đang có Job Application."
                });
            }

            // 3️⃣ Xóa
            await _service.Delete(id);

            return Ok(new
            {
                success = true,
                message = "Xóa vị trí công việc thành công."
            });
        }

    }
}