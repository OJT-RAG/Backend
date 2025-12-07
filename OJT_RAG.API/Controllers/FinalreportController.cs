using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.FinalreportDTO;
using OJT_RAG.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinalreportController : ControllerBase
    {
        private readonly IFinalreportService _service;

        public FinalreportController(IFinalreportService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách final report thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách final report.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetById(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy final report." })
                    : Ok(new { message = "Lấy final report thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy final report với Id = {id}.", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(long userId)
        {
            try
            {
                var result = await _service.GetByUser(userId);
                return Ok(new { message = "Lấy final report theo user thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy final report của user Id = {userId}.", error = ex.Message });
            }
        }

        [HttpGet("semester/{semesterId}")]
        public async Task<IActionResult> GetBySemester(long semesterId)
        {
            try
            {
                var result = await _service.GetBySemester(semesterId);
                return Ok(new { message = "Lấy final report theo semester thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy final report của semester Id = {semesterId}.", error = ex.Message });
            }
        }

        [HttpGet("job/{jobPositionId}")]
        public async Task<IActionResult> GetByJob(long jobPositionId)
        {
            try
            {
                var result = await _service.GetByJob(jobPositionId);
                return Ok(new { message = "Lấy final report theo job position thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy final report của job position Id = {jobPositionId}.", error = ex.Message });

            }
        }

        // 🔥 FIX HERE: Dùng FromForm để hỗ trợ upload file
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateFinalreportDTO dto)
        {
            try
            {
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo final report thành công.", data = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
                throw;
            }
        }

        // 🔥 FIX HERE: Dùng FromForm hỗ trợ upload file khi update
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateFinalreportDTO dto)
        {
            try
            {
                var result = await _service.Update(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy final report để cập nhật." })
                    : Ok(new { message = "Cập nhật final report thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật final report.", error = ex.Message });
            }
        }
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var success = await _service.Delete(id);
                return success
                    ? Ok(new { message = "Xóa final report thành công." })
                    : NotFound(new { message = "Không tìm thấy final report để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa final report với Id = {id}.", error = ex.Message });
            }
        }
    }
}
