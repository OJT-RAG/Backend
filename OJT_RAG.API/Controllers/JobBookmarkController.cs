using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.JobBookmarkDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobBookmarkController : ControllerBase
    {
        private readonly IJobBookmarkService _service;

        public JobBookmarkController(IJobBookmarkService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách job bookmark thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách job bookmark.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetById(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy job bookmark." })
                    : Ok(new { message = "Lấy job bookmark thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy job bookmark với Id = {id}.", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(long userId)
        {
            try
            {
                var result = await _service.GetByUserId(userId);
                return Ok(new { message = "Lấy danh sách job bookmark theo user thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy job bookmark của user Id = {userId}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateJobBookmarkDTO model)
        {
            try
            {
                var result = await _service.Create(model);
                return Ok(new { message = "Tạo job bookmark thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest(new { message = "Job bookmark đã tồn tại (userId + jobId trùng)." });
                }

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo job bookmark.", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateJobBookmarkDTO model)
        {
            try
            {
                var result = await _service.Update(model);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy job bookmark để cập nhật." })
                    : Ok(new { message = "Cập nhật job bookmark thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị bị trùng với job bookmark khác." });
                }

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật job bookmark.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _service.Delete(id);
                return result
                    ? Ok(new { message = "Xóa job bookmark thành công." })
                    : NotFound(new { message = "Không tìm thấy job bookmark để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa job bookmark với Id = {id}.", error = ex.Message });
            }
        }
    }
}
