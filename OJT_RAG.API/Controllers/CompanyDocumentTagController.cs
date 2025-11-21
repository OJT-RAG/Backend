using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.CompanyDocumentTag;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/company-document-tag")]
    public class CompanyDocumentTagController : ControllerBase
    {
        private readonly ICompanyDocumentTagService _service;

        public CompanyDocumentTagController(ICompanyDocumentTagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi lấy danh sách.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetById(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy bản ghi." })
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Đã xảy ra lỗi khi lấy dữ liệu với Id = {id}.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDocumentTagDTO dto)
        {
            try
            {
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo mới thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Tạo thất bại: dữ liệu bị trùng (duplicate key)." });
                }

                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi tạo mới.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyDocumentTagDTO dto)
        {
            try
            {
                var result = await _service.Update(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy bản ghi để cập nhật." })
                    : Ok(new { message = "Cập nhật thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: dữ liệu bị trùng." });
                }

                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi cập nhật.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.Delete(id);
                return ok
                    ? Ok(new { message = "Xóa thành công." })
                    : NotFound(new { message = "Không tìm thấy bản ghi để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Đã xảy ra lỗi khi xóa bản ghi với Id = {id}.",
                    error = ex.Message
                });
            }
        }
    }
}
