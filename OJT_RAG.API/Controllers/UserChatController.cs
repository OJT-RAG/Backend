using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.ChatDTO;
using OJT_RAG.Services;
using OJT_RAG.Services.Exceptions;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/user-chat")]
    public class UserChatController : ControllerBase
    {
        private readonly UserChatService _service;

        public UserChatController(UserChatService service)
        {
            _service = service;
        }

        // ================= SEND MESSAGE =================
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendMessageDTO dto)
        {
            // 🔒 Validate request body
            if (dto == null)
            {
                return BadRequest(new
                {
                    message = "Dữ liệu gửi lên không hợp lệ"
                });
            }

            if (dto.SenderId <= 0 || dto.ReceiverId <= 0)
            {
                return BadRequest(new
                {
                    message = "SenderId và ReceiverId phải lớn hơn 0"
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                return BadRequest(new
                {
                    message = "Nội dung tin nhắn không được rỗng"
                });
            }

            try
            {
                var msg = await _service.SendMessage(
                    dto.SenderId,
                    dto.ReceiverId,
                    dto.Content
                );

                return Ok(new
                {
                    message = "Gửi tin nhắn thành công",
                    data = msg
                });
            }
            catch (UserNotFoundException ex)
            {
                // ❌ User không tồn tại
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                // ❌ Lỗi validate logic (chat với chính mình, content rỗng...)
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // ❌ Lỗi hệ thống
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi gửi tin nhắn",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // ================= GET CONVERSATION =================
        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversation(
            [FromQuery] long user1,
            [FromQuery] long user2)
        {
            // 🔒 Validate query
            if (user1 <= 0 || user2 <= 0)
            {
                return BadRequest(new
                {
                    message = "user1 và user2 phải lớn hơn 0"
                });
            }

            if (user1 == user2)
            {
                return BadRequest(new
                {
                    message = "Không thể lấy hội thoại với chính mình"
                });
            }

            try
            {
                var data = await _service.GetConversation(user1, user2);

                return Ok(new
                {
                    message = "Lấy hội thoại thành công",
                    data = data
                });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống khi lấy hội thoại",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
