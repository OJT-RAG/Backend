using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.CompanyDocumentDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDocumentController : ControllerBase
    {
        private readonly ICompanyDocumentService _service;

        public CompanyDocumentController(ICompanyDocumentService service)
        {
            _service = service;
        }


        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách tài liệu công ty thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi lấy danh sách tài liệu.",
                    error = ex.Message
                });
            }
        }


        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetById(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy tài liệu công ty." })
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Đã xảy ra lỗi khi lấy tài liệu có Id = {id}.",
                    error = ex.Message
                });
            }
        }


        [HttpGet("getBySemester/{semCompanyId}")]
        public async Task<IActionResult> GetBySemester(long semCompanyId)
        {
            try
            {
                var result = await _service.GetBySemester(semCompanyId);
                return Ok(new { message = "Lấy tài liệu theo SemesterCompanyId thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Đã xảy ra lỗi khi lấy tài liệu cho SemesterCompanyId = {semCompanyId}.",
                    error = ex.Message
                });
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateCompanyDocumentDTO dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (long.TryParse(userIdClaim, out long userId))
                {
                    dto.UploadedBy = userId; // Backend tự gán 7, bất kể user nhập gì hoặc để trống
                }

                // Tiếp tục logic xử lý...
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo tài liệu thành công với ID người dùng: " + dto.UploadedBy });
            }
            catch (Exception ex)
            {
                var realErrorMessage = ex.InnerException?.Message ?? ex.Message;

                // Bắt lỗi 23502: Cột ID hoặc các cột NOT NULL bị trống (Lỗi Database)
                if (realErrorMessage.Contains("23502") || realErrorMessage.Contains("null value"))
                {
                    return BadRequest(new
                    {
                        message = "Lỗi Database: Cột ID chưa được thiết lập Identity (Tự tăng) hoặc thiếu dữ liệu bắt buộc.",
                        detail = realErrorMessage
                    });
                }

                // Bắt lỗi 23503: Khóa ngoại (SemesterCompanyId hoặc UploadedBy không tồn tại)
                if (realErrorMessage.Contains("23503") || realErrorMessage.Contains("foreign key"))
                {
                    return BadRequest(new
                    {
                        message = "Lỗi ràng buộc: SemesterCompanyId không tồn tại hoặc ID người dùng không hợp lệ.",
                        detail = realErrorMessage
                    });
                }

                // Lỗi trùng lặp
                if (realErrorMessage.Contains("23505") || realErrorMessage.Contains("duplicate"))
                {
                    return BadRequest(new { message = "Tài liệu này đã tồn tại.", detail = realErrorMessage });
                }

                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi hệ thống khi tạo tài liệu.",
                    error = ex.Message,
                    innerError = realErrorMessage
                });
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateCompanyDocumentDTO dto)
        {
            try
            {
                var result = await _service.Update(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy tài liệu để cập nhật." })
                    : Ok(new { message = "Cập nhật tài liệu thành công.", data = result });
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
                    message = "Đã xảy ra lỗi khi cập nhật tài liệu.",
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
                var ok = await _service.Delete(id);
                return ok
                    ? Ok(new { message = "Xóa tài liệu công ty thành công." })
                    : NotFound(new { message = "Không tìm thấy tài liệu để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Đã xảy ra lỗi khi xóa tài liệu có Id = {id}.",
                    error = ex.Message
                });
            }
        }

        [Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(long id)
        {
            var result = await _service.Download(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy tài liệu." });

            return File(
                result.Value.fileBytes,
                result.Value.contentType,
                result.Value.fileName
            );
        }

        [HttpGet("{id}/tags")]
        public async Task<IActionResult> GetTags(long id)
        {
            var tags = await _service.GetTags(id);
            return Ok(tags);
        }

        [HttpPost("{id}/tags")]
        public async Task<IActionResult> AddTag(long id, [FromBody] long tagId)
        {
            await _service.AddTag(id, tagId);
            return Ok();
        }

        [HttpDelete("{id}/tags/{tagId}")]
        public async Task<IActionResult> RemoveTag(long id, long tagId)
        {
            await _service.RemoveTag(id, tagId);
            return NoContent();
        }

    }
}
