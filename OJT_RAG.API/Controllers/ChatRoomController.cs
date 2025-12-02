using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.ChatRoomDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatRoomController : ControllerBase
    {
        private readonly IChatRoomService _service;

        public ChatRoomController(IChatRoomService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách phòng chat thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách phòng chat.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetById(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy phòng chat." })
                    : Ok(new { message = "Lấy phòng chat thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy phòng chat với Id = {id}.", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(long userId)
        {
            try
            {
                var result = await _service.GetByUser(userId);
                return Ok(new { message = "Lấy danh sách phòng chat theo user thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy phòng chat của user Id = {userId}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateChatRoomDTO dto)
        {
            try
            {
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo phòng chat thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest(new { message = "Phòng chat đã tồn tại (trùng dữ liệu unique)." });
                }

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo phòng chat.", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateChatRoomDTO dto)
        {
            try
            {
                var result = await _service.Update(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy phòng chat để cập nhật." })
                    : Ok(new { message = "Cập nhật phòng chat thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: dữ liệu bị trùng với phòng chat khác." });
                }

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật phòng chat.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var success = await _service.Delete(id);
                return success
                    ? Ok(new { message = "Xóa phòng chat thành công." })
                    : NotFound(new { message = "Không tìm thấy phòng chat để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa phòng chat với Id = {id}.", error = ex.Message });
            }
        }
    }
}
