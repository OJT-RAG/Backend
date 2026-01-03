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
    public async Task<IActionResult> GoogleLogin(
        [FromBody] GoogleLoginRequestDTO dto)
    {
        try
        {
            var token = await _googleAuth.LoginWithGoogleAsync(dto.IdToken);

            return Ok(new
            {
                message = "Google login thành công",
                accessToken = token
            });
        }
        catch (DbUpdateException ex)
        {
            var detail = ex.InnerException?.Message;
            return BadRequest(new
            {
                message = "DB ERROR",
                detail
            });
        }

    }
}
