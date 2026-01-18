using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.OjtDocumentDTO;
using OJT_RAG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OJT_RAG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OjtDocumentController : ControllerBase
    {
        private readonly IOjtDocumentService _service;

        public OjtDocumentController(IOjtDocumentService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(new { message = "Lấy danh sách tài liệu OJT thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi lấy danh sách tài liệu OJT.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                return item == null
                    ? NotFound(new { message = "Không tìm thấy tài liệu OJT." })
                    : Ok(new { message = "Lấy tài liệu OJT thành công.", data = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Đã xảy ra lỗi khi lấy tài liệu OJT có Id = {id}.",
                    error = ex.Message
                });
            }
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateOjtDocumentDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(new { message = "Tạo tài liệu OJT thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Foreign key violated
                    if (ex.InnerException.Message.Contains("foreign key"))
                        return BadRequest(new { message = "Tham chiếu khóa ngoại không tồn tại." });

                    // Duplicate key
                    if (ex.InnerException.Message.Contains("duplicate"))
                        return BadRequest(new { message = "Tài liệu đã tồn tại." });
                }

                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi tạo tài liệu OJT.",
                    error = ex.Message
                });
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateOjtDocumentDTO dto)
        {
            try
            {
                var result = await _service.UpdateAsync(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy tài liệu OJT để cập nhật." })
                    : Ok(new { message = "Cập nhật tài liệu OJT thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("duplicate"))
                        return BadRequest(new { message = "Tên tài liệu hoặc trường unique bị trùng." });
                }

                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi cập nhật tài liệu OJT.",
                    error = ex.Message
                });
            }
        }
       [Authorize]
[HttpDelete("delete/{id}")]
public async Task<IActionResult> Delete(long id)
{
    try
    {
        var success = await _service.DeleteAsync(id);
        return success
            ? Ok(new { message = "Xóa tài liệu OJT thành công." })
            : NotFound(new { message = "Không tìm thấy tài liệu OJT để xóa." });
    }
    catch (DbUpdateException ex)
    {
        // 🔴 LỖI FK: document đang gắn tag
        if (ex.InnerException != null &&
            ex.InnerException.Message.Contains("foreign key"))
        {
            return BadRequest(new
            {
                message = "Không thể xóa tài liệu vì đang được gắn với thẻ (OjtDocumentTag)."
            });
        }

        return StatusCode(500, new
        {
            message = "Lỗi database khi xóa tài liệu OJT.",
            error = ex.InnerException?.Message ?? ex.Message
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = $"Đã xảy ra lỗi khi xóa tài liệu OJT có Id = {id}.",
            error = ex.Message
        });
    }
}

        [Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(long id)
        {
            var result = await _service.DownloadAsync(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy tài liệu OJT." });

            return File(
                result.Value.fileBytes,
                result.Value.contentType,
                result.Value.fileName
            );
        }

    }
}
