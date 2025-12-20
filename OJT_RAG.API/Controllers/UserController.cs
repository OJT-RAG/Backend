using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.UserDTO;
using OJT_RAG.Services.Auth;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly JwtService _jwt;

        public UserController(IUserService service, JwtService jwt)
        {
            _service = service;
            _jwt = jwt;
        }

        // ------------------------- LOGIN ----------------------------
        [HttpPost("login")]
        [AllowAnonymous] // không cần token
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            var user = await _service.Login(dto.Email, dto.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });
            }

            // 🎯 tạo JWT bằng JwtService inject từ DI
            var token = _jwt.GenerateToken(user.UserId, user.Email/*, user.Role*/);

            return Ok(new
            {
                message = "Đăng nhập thành công.",
                token = token,
                data = user
            });
        }


        // ------------------------- GET ALL ----------------------------
        [HttpGet("getAll")]
       
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(new { message = "Lấy danh sách người dùng thành công.", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi lấy danh sách người dùng.",
                    error = ex.Message
                });
            }
        }


        // ------------------------- GET BY ID ----------------------------
        [HttpGet("get/{id}")]
   
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var user = await _service.GetById(id);
                return user == null
                    ? NotFound(new { message = $"Không tìm thấy người dùng với Id = {id}." })
                    : Ok(new { message = "Lấy người dùng thành công.", data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Đã xảy ra lỗi khi lấy người dùng với Id = {id}.",
                    error = ex.Message
                });
            }
        }


        // ------------------------- CREATE ----------------------------
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
        {
            try
            {
                var result = await _service.Create(dto);
                return Ok(new { message = "Tạo người dùng thành công.", data = result });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    return BadRequest(new
                    {
                        message = "Người dùng đã tồn tại (Id hoặc trường unique bị trùng)."
                    });
                }

                Console.WriteLine("ERROR: " + ex.ToString());
                throw;
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


        // ------------------------- DELETE ----------------------------
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.Delete(id);
                return ok
                    ? Ok(new { message = "Xóa người dùng thành công." })
                    : NotFound(new { message = "Không tìm thấy người dùng để xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Đã xảy ra lỗi khi xóa người dùng với Id = {id}.",
                    error = ex.Message
                });
            }
        }
    }
}
