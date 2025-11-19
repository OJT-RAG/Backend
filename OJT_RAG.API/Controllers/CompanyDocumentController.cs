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
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo tài liệu công ty thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Foreign key violated
                    if (ex.InnerException.Message.Contains("foreign key"))
                        return BadRequest(new { message = "SemesterCompanyId không tồn tại." });

                    // Duplicate key
                    if (ex.InnerException.Message.Contains("duplicate"))
                        return BadRequest(new { message = "Tài liệu đã tồn tại." });
                }

                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi tạo tài liệu.",
                    error = ex.Message
                });
            }
        }

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
    }
}
