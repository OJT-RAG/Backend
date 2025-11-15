using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.JobTitleOverviewDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/job-title-overview")]
    [ApiController]
    public class JobTitleOverviewController : ControllerBase
    {
        private readonly IJobTitleOverviewService _service;

        public JobTitleOverviewController(IJobTitleOverviewService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách Job Title Overview thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách Job Title Overview.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _service.GetById(id);
                return data == null
                    ? NotFound(new { message = $"Không tìm thấy Job Title Overview với Id = {id}." })
                    : Ok(new { message = "Lấy Job Title Overview thành công.", data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy Job Title Overview với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateJobTitleOverviewDTO dto)
        {
            try
            {
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo Job Title Overview thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Job Title Overview đã tồn tại (Id hoặc trường unique bị trùng)." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo Job Title Overview.", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateJobTitleOverviewDTO dto)
        {
            try
            {
                var result = await _service.Update(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy Job Title Overview để cập nhật." })
                    : Ok(new { message = "Cập nhật Job Title Overview thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với Job Title Overview khác." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật Job Title Overview.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.Delete(id);
                return ok
                    ? Ok(new { message = "Xóa Job Title Overview thành công." })
                    : NotFound(new { message = "Không tìm thấy Job Title Overview để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa Job Title Overview với Id = {id}.", error = ex.Message });
            }
        }
    }
}
