using Microsoft.AspNetCore.Mvc;
using OJT_RAG.ModelViews.Major;
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
                return Ok(new { message = "Lấy danh sách chuyên ngành thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách chuyên ngành.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy chuyên ngành." })
                    : Ok(new { message = "Lấy chuyên ngành thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy chuyên ngành với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MajorCreateModel dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(new { message = "Tạo chuyên ngành thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Chuyên ngành đã tồn tại." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo chuyên ngành.", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] MajorUpdateModel dto)
        {
            try
            {
                var result = await _service.UpdateAsync(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy chuyên ngành để cập nhật." })
                    : Ok(new { message = "Cập nhật chuyên ngành thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với chuyên ngành khác." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật chuyên ngành.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                return ok
                    ? Ok(new { message = "Xóa chuyên ngành thành công." })
                    : NotFound(new { message = "Không tìm thấy chuyên ngành để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa chuyên ngành với Id = {id}.", error = ex.Message });
            }
        }
    }
}
