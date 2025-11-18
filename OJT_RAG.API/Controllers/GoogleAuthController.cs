using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Util;

[ApiController]
[Route("auth/google")]
public class GoogleAuthController : ControllerBase
{
    [HttpGet("callback")]
    public async Task<IActionResult> Callback(string code)
    {
        var secrets = GoogleClientSecrets.FromFile("client_secret.json");

        var tokenRequest = new AuthorizationCodeTokenRequest
        {
            ClientId = secrets.Secrets.ClientId,
            ClientSecret = secrets.Secrets.ClientSecret,
            Code = code,
            RedirectUri = "https://localhost:7031/auth/google/callback",
            GrantType = "authorization_code"
        };

        var response = await tokenRequest.ExecuteAsync(
            new HttpClient(),
            "https://oauth2.googleapis.com/token",
            CancellationToken.None,
            SystemClock.Default
        );

        if (response.RefreshToken == null)
        {
            return BadRequest("No refresh token received. Try adding prompt=consent in login URL.");
        }

        // ✅ SỬ DỤNG System.IO.File để tránh nhầm với ControllerBase.File
        System.IO.File.WriteAllText("refresh_token.txt", response.RefreshToken);

        return Ok("Google Drive connected successfully! Refresh token saved.");
    }


}
