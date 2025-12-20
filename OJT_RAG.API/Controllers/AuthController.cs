using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.Auth;

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
                message = "Login Google thành công",
                accessToken = token
            });
        }
        catch (Exception ex)
        {
            return Unauthorized(new
            {
                message = "Google login failed",
                error = ex.Message
            });
        }
    }
}
