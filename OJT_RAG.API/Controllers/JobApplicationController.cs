using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Services.DTOs.JobApplication;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _service;

        public JobApplicationController(IJobApplicationService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(new { message = "Lấy danh sách đơn ứng tuyển thành công", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateJobApplicationDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(new { message = "Ứng tuyển thành công", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ứng tuyển thất bại", error = ex.Message });
            }
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateJobApplicationStatusDTO dto)
        {
            try
            {
                var result = await _service.UpdateStatusAsync(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy đơn ứng tuyển" })
                    : Ok(new { message = "Cập nhật trạng thái thành công", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }
    }
}
