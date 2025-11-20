using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.DocumentTagDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/document-tag")]
    [ApiController]
    public class DocumentTagController : ControllerBase
    {
        private readonly IDocumentTagService _service;

        public DocumentTagController(IDocumentTagService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách tag tài liệu thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách tag.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var result = await _service.GetById(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy tag tài liệu." })
                    : Ok(new { message = "Lấy tag tài liệu thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy tag với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDocumentTagDTO dto)
        {
            try
            {
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo tag tài liệu thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Tag đã tồn tại (trùng tên hoặc trường unique)." });
                }

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo tag tài liệu.", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDocumentTagDTO dto)
        {
            try
            {
                var result = await _service.Update(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy tag để cập nhật." })
                    : Ok(new { message = "Cập nhật tag tài liệu thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với tag khác." });
                }

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật tag tài liệu.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.Delete(id);

                return ok
                    ? Ok(new { message = "Xóa tag tài liệu thành công." })
                    : NotFound(new { message = "Không tìm thấy tag để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa tag với Id = {id}.", error = ex.Message });
            }
        }
    }
}
