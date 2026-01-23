using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OJT_RAG.DTOs.Auth;
using OJT_RAG.Services.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly GoogleAuthService _googleAuth;

    public AuthController(GoogleAuthService googleAuth)
    {
        _googleAuth = googleAuth;
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO dto)
    {
        try
        {
            // Gọi service và nhận về cả Token lẫn UserInfo
            var (token, userInfo) = await _googleAuth.LoginWithGoogleAsync(dto.IdToken);

            return Ok(new
            {
                message = "Google login thành công",
                accessToken = token,
                user = userInfo
            });
        }
        catch (InvalidJwtException ex)
        {
            return BadRequest(new { message = "Google Token không hợp lệ", detail = ex.Message });
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new { message = "Lỗi lưu dữ liệu database", detail = ex.InnerException?.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Đã có lỗi xảy ra", detail = ex.Message });
        }
    }

}

