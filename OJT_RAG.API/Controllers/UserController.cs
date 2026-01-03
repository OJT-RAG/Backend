using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.UserDTO;
using OJT_RAG.Services.Auth;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly JwtService _jwt;

        public UserController(IUserService service, JwtService jwt)
        {
            _service = service;
            _jwt = jwt;
        }

        // ================= LOGIN =================
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            try
            {
                var user = await _service.Login(dto.Email, dto.Password);
                if (user == null)
                    return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });

                var token = _jwt.GenerateToken(user.UserId, user.Email);

                return Ok(new { token, data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ================= GET ALL =================
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ================= GET BY ID =================
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var user = await _service.GetById(id);
                return user == null ? NotFound() : Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ================= CREATE (UPLOAD CV + AVATAR) =================
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateUserDTO dto)
        {
            try
            {
                await _service.Create(dto);
                return Ok(new { message = "Tạo user thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // ------------------------- UPDATE ----------------------------
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateUserDTO dto)
        {
            try
            {
                var result = await _service.Update(dto);
                return result == null
                    ? NotFound(new { message = "Không tìm thấy người dùng để cập nhật." })
                    : Ok(new { message = "Cập nhật người dùng thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new
                    {
                        message = "Cập nhật thất bại: giá trị trùng với người dùng khác."
                    });
                }

                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi cập nhật người dùng.",
                    error = ex.Message
                });
            }
        }

        // ================= DELETE =================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
