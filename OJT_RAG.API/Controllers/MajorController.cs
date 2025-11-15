using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Services.DTOs.Major;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly IMajorService _service;

        public MajorController(IMajorService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(new { message = "Lấy danh sách ngành học thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách ngành học.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy ngành học." })
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy ngành học với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateMajorDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(new { message = "Tạo ngành học thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Ngành học đã tồn tại (Id hoặc trường unique bị trùng)." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo ngành học.", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateMajorDTO dto)
        {
            try
            {
                var result = await _service.UpdateAsync(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy ngành học để cập nhật." })
                    : Ok(new { message = "Cập nhật ngành học thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với ngành học khác." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật ngành học.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                return ok
                    ? Ok(new { message = "Xóa ngành học thành công." })
                    : NotFound(new { message = "Không tìm thấy ngành học để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa ngành học với Id = {id}.", error = ex.Message });
            }
        }
    }
}
