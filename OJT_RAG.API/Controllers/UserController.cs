using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.UserDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    return BadRequest(new { message = "Email và mật khẩu là bắt buộc." });
                }

                var user = await _service.Login(dto.Email, dto.Password);
                if (user == null)
                {
                    return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });
                }

                return Ok(new { message = "Đăng nhập thành công.", data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng nhập.", error = ex.Message });
            }
        }

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
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách người dùng.", error = ex.Message });
            }
        }

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
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi lấy người dùng với Id = {id}.", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUserDTO dto)
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
                    return BadRequest(new { message = "Người dùng đã tồn tại (Id hoặc trường unique bị trùng)." });
                }
                Console.WriteLine("ERROR: " + ex.ToString());
                throw;
            }
        }

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
                    return BadRequest(new { message = "Cập nhật thất bại: giá trị trùng với người dùng khác." });
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật người dùng.", error = ex.Message });
            }
        }

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
                return StatusCode(500, new { message = $"Đã xảy ra lỗi khi xóa người dùng với Id = {id}.", error = ex.Message });
            }
        }
    }
}
