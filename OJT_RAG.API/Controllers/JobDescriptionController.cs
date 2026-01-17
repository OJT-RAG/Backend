using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OJT_RAG.Services.DTOs.JobDescription;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobDescriptionController : ControllerBase
    {
        private readonly IJobDescriptionService _service;

        public JobDescriptionController(IJobDescriptionService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(new { message = "Lấy danh sách mô tả công việc thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách mô tả công việc.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? NotFound(new { message = "Không tìm thấy mô tả công việc." }) : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy mô tả công việc với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateJobDescriptionDTO dto)
        {
            // Debug: Log DTO nhận được
            Console.WriteLine($"DTO received: JobPositionId={dto.JobPositionId}, JobDescription={(dto.JobDescription ?? "NULL")}, Hire={dto.HireQuantity}, Applied={dto.AppliedQuantity}");

            if (dto == null)
            {
                return BadRequest("DTO bị null - binding thất bại");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return BadRequest(new { message = "Validation fail", errors });
            }

            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(new { message = "Tạo thành công", data = result });
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { message = "DB error", detail = inner });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", detail = ex.Message, stack = ex.StackTrace });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateJobDescriptionDTO dto)
        {
            try
            {
                var result = await _service.UpdateAsync(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy mô tả công việc để cập nhật." })
                    : Ok(new { message = "Cập nhật mô tả công việc thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với mô tả công việc khác." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật mô tả công việc.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                return ok
                    ? Ok(new { message = "Xóa mô tả công việc thành công." })
                    : NotFound(new { message = "Không tìm thấy mô tả công việc để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa mô tả công việc với Id = {id}.", error = ex.Message });
            }
        }
    }
}
