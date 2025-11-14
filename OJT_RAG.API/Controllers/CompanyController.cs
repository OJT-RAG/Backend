using Microsoft.AspNetCore.Mvc;
using OJT_RAG.ModelViews.Company;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(new { message = "Lấy danh sách công ty thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách công ty.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? NotFound(new { message = "Không tìm thấy công ty." }) : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy công ty với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CompanyCreateModel model)
        {
            try
            {
                var result = await _service.CreateAsync(model);
                return Ok(new { message = "Tạo công ty thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Công ty đã tồn tại (Id hoặc trường unique bị trùng)." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo công ty.", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] CompanyUpdateModel model)
        {
            try
            {
                var result = await _service.UpdateAsync(model);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy công ty để cập nhật." })
                    : Ok(new { message = "Cập nhật công ty thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với công ty khác." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật công ty.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                return ok
                    ? Ok(new { message = "Xóa công ty thành công." })
                    : NotFound(new { message = "Không tìm thấy công ty để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa công ty với Id = {id}.", error = ex.Message });
            }
        }
    }
}
