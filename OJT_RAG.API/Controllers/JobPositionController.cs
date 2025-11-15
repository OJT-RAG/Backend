using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.JobPositionDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPositionController : ControllerBase
    {
        private readonly IJobPositionService _service;

        public JobPositionController(IJobPositionService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(new { message = "Lấy danh sách vị trí công việc thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách vị trí công việc.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _service.GetByIdAsync(id);
                return data == null
                    ? NotFound(new { message = "Không tìm thấy vị trí công việc." })
                    : Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy vị trí công việc với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] JobPositionCreateDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(new { message = "Tạo vị trí công việc thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Vị trí công việc đã tồn tại (Id hoặc trường unique bị trùng)." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo vị trí công việc.", error = ex.Message });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] JobPositionUpdateDTO dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy vị trí công việc để cập nhật." })
                    : Ok(new { message = "Cập nhật vị trí công việc thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với vị trí công việc khác." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật vị trí công việc.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                return ok
                    ? Ok(new { message = "Xóa vị trí công việc thành công." })
                    : NotFound(new { message = "Không tìm thấy vị trí công việc để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa vị trí công việc với Id = {id}.", error = ex.Message });
            }
        }
    }
}
