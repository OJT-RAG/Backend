using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.MessageDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;

        public MessageController(IMessageService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách tin nhắn thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách tin nhắn.", error = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetById(id);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy tin nhắn." })
                    : Ok(new { message = "Lấy tin nhắn thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy tin nhắn với Id = {id}.", error = ex.Message });
            }
        }

        [HttpGet("chat-room/{chatRoomId}")]
        public async Task<IActionResult> GetByChatRoom(long chatRoomId)
        {
            try
            {
                var result = await _service.GetByChatRoom(chatRoomId);
                return Ok(new { message = "Lấy tin nhắn theo phòng chat thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy tin nhắn của phòng chat Id = {chatRoomId}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateMessageDTO dto)
        {
            try
            {
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo tin nhắn thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest(new { message = "Tin nhắn đã tồn tại (trùng dữ liệu unique)." });
                }

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo tin nhắn.", error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateMessageDTO dto)
        {
            try
            {
                var result = await _service.Update(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy tin nhắn để cập nhật." })
                    : Ok(new { message = "Cập nhật tin nhắn thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest(new { message = "Cập nhật thất bại: dữ liệu trùng với tin nhắn khác." });
                }

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật tin nhắn.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var success = await _service.Delete(id);
                return success
                    ? Ok(new { message = "Xóa tin nhắn thành công." })
                    : NotFound(new { message = "Không tìm thấy tin nhắn để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa tin nhắn với Id = {id}.", error = ex.Message });
            }
        }
    }
}
