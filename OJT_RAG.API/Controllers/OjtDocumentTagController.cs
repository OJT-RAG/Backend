using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.OjtDocumentTag;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/ojt-document-tag")]
    public class OjtDocumentTagController : ControllerBase
    {
        private readonly IOjtDocumentTagService _service;

        public OjtDocumentTagController(IOjtDocumentTagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách thành công.", data = list });
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
                var item = await _service.GetById(id);
                return item == null
                    ? NotFound(new { message = "Không tìm thấy bản ghi." })
                    : Ok(item);
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
        public async Task<IActionResult> Create([FromBody] CreateOjtDocumentTagDTO dto)
        {
            try
            {
                var created = await _service.Create(dto);
                return Ok(new { message = "Tạo mới thành công.", data = created });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.Message.Contains("duplicate key"))
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
        public async Task<IActionResult> Update([FromBody] UpdateOjtDocumentTagDTO dto)
        {
            try
            {
                var updated = await _service.Update(dto);
                return updated == null
                    ? NotFound(new { message = "Không tìm thấy bản ghi để cập nhật." })
                    : Ok(new { message = "Cập nhật thành công.", data = updated });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.Message.Contains("duplicate key"))
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
