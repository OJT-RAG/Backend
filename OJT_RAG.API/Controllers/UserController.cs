using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.UserDTO;
using OJT_RAG.Services.Auth;
using OJT_RAG.Services.DTOs.User;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly JwtService _jwt;
        private long GetCurrentUserId()
        {
            return long.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
        }
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _service.Login(dto.Email, dto.Password);
                if (user == null)
                    return Unauthorized(new { success = false, message = "Email hoặc mật khẩu không đúng." });

                var token = _jwt.GenerateToken(user.UserId, user.Email);

                return Ok(new { success = true, token, data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống khi đăng nhập.", error = ex.Message });
            }
        }

        // ================= GET ALL =================
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _service.GetAll();
                return Ok(new { success = true, data = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Không thể lấy danh sách người dùng.", error = ex.Message });
            }
        }

        // ================= GET BY ID =================
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var user = await _service.GetById(id);
                if (user == null)
                    return NotFound(new { success = false, message = $"Không tìm thấy người dùng với ID {id}" });

                return Ok(new { success = true, data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi truy xuất thông tin người dùng.", error = ex.Message });
            }
        }

        // ================= CREATE (UPLOAD CV + AVATAR) =================
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Giả sử service trả về user vừa tạo hoặc kiểm tra email trùng trong service
                await _service.Create(dto);
                return CreatedAtAction(nameof(Get), new { id = "new_user" }, new { success = true, message = "Tạo người dùng thành công." });
            }
            catch (ArgumentException ex) // Bắt các lỗi nghiệp vụ như Email đã tồn tại
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi tạo người dùng.", error = ex.Message });
            }
        }

        // ------------------------- UPDATE ----------------------------
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.Update(dto);
                if (result == null)
                    return NotFound(new { success = false, message = "Cập nhật thất bại: Không tìm thấy người dùng." });

                return Ok(new { success = true, message = "Cập nhật người dùng thành công.", data = result });
            }
            catch (Exception ex)
            {
                // Kiểm tra lỗi trùng Unique Key (Email/Phone) từ Database
                if (ex.InnerException?.Message.Contains("duplicate") == true || ex.Message.Contains("duplicate"))
                {
                    return Conflict(new { success = false, message = "Dữ liệu cập nhật (Email/SĐT) đã được sử dụng bởi người khác." });
                }

                return StatusCode(500, new { success = false, message = "Lỗi server khi cập nhật.", error = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpPut("update-status")]
        // [Authorize(Roles = "Admin")] // bật nếu cần
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateUserStatusDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = GetCurrentUserId();

            // 🚫 KHÔNG cho tự update status bản thân
            if (dto.UserId == currentUserId)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Không được cập nhật trạng thái tài khoản của chính mình."
                });
            }

            var updated = await _service.UpdateStatus(dto);

            if (!updated)
                return NotFound(new { success = false, message = "Không tìm thấy người dùng." });

            return Ok(new
            {
                success = true,
                message = $"Cập nhật trạng thái thành '{dto.AccountStatus}' thành công."
            });
        }

        // ================= DELETE =================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var user = await _service.GetById(id);
                if (user == null)
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng để xóa." });

                await _service.Delete(id);
                return Ok(new { success = true, message = $"Đã xóa người dùng ID {id} thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi xóa người dùng.", error = ex.Message });
            }
        }

    }
}
