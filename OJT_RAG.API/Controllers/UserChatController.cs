using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.ChatDTO;
using OJT_RAG.Services;

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

        // ============ SEND MESSAGE ============
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendMessageDTO dto)
        {
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Gửi tin nhắn thất bại",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // ============ GET CONVERSATION ============
        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversation([FromQuery] long user1, [FromQuery] long user2)
        {
            try
            {
                var data = await _service.GetConversation(user1, user2);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi lấy hội thoại",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
