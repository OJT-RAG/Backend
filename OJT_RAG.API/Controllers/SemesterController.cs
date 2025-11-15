using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.SemesterDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemesterController(ISemesterService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(new { message = "Lấy danh sách học kỳ thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách học kỳ.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _service.GetByIdAsync(id);
                return data == null
                    ? NotFound(new { message = "Không tìm thấy học kỳ." })
                    : Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy học kỳ với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SemesterCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(new { message = "Tạo học kỳ thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Học kỳ đã tồn tại (Id hoặc trường unique bị trùng)." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo học kỳ.", error = ex.Message });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] SemesterUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            try
            {
                var result = await _service.UpdateAsync(id, dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy học kỳ để cập nhật." })
                    : Ok(new { message = "Cập nhật học kỳ thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với học kỳ khác." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật học kỳ.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                return ok
                    ? Ok(new { message = "Xóa học kỳ thành công." })
                    : NotFound(new { message = "Không tìm thấy học kỳ để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa học kỳ với Id = {id}.", error = ex.Message });
            }
        }
    }
}
