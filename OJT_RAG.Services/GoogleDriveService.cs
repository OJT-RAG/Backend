using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;

public class GoogleDriveService
{
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var refreshToken = File.ReadAllText("refresh_token.txt");
        var secrets = GoogleClientSecrets.FromFile("client_secret.json");

        var credential = new UserCredential(
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets.Secrets,
                Scopes = new[] { DriveService.Scope.DriveFile }
            }),
            "admin",
            new TokenResponse { RefreshToken = refreshToken }
        );

        var drive = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "OJT_RAG"
        });

        var metadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = file.FileName
        };

        var request = drive.Files.Create(metadata, file.OpenReadStream(), file.ContentType);
        request.Fields = "id";

        await request.UploadAsync();

        return request.ResponseBody.Id;
    }
}
