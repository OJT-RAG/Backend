using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;

public class GoogleDriveService
{
    private readonly string _clientSecretPath = "client_secret.json";
    private readonly string _refreshTokenPath = "refresh_token.txt";

    private async Task<UserCredential> GetCredentialAsync()
    {
        var refreshToken = await File.ReadAllTextAsync(_refreshTokenPath);
        var secrets = GoogleClientSecrets.FromFile(_clientSecretPath);

        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = secrets.Secrets,
            Scopes = new[] { DriveService.Scope.DriveFile }
        });

        return new UserCredential(flow, "system-user", new TokenResponse { RefreshToken = refreshToken });
    }

    private async Task<DriveService> GetDriveServiceAsync()
    {
        var credential = await GetCredentialAsync();
        return new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "OJT_RAG"
        });
    }

    // ---- NEW VERSION: Get or create folder with parent ----
    public async Task<string> GetOrCreateFolderAsync(string folderName, string? parentId = null)
    {
        var drive = await GetDriveServiceAsync();

        string q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName.Replace("'", "\\'")}' and trashed=false";

        if (parentId != null)
            q += $" and '{parentId}' in parents";

        var listReq = drive.Files.List();
        listReq.Q = q;
        listReq.Fields = "files(id, name)";
        var list = await listReq.ExecuteAsync();

        if (list.Files.Count > 0)
            return list.Files[0].Id;

        var metadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = folderName,
            MimeType = "application/vnd.google-apps.folder",
            Parents = parentId != null ? new List<string> { parentId } : null
        };

        var createReq = drive.Files.Create(metadata);
        createReq.Fields = "id";
        var created = await createReq.ExecuteAsync();

        return created.Id;
    }

    // ---- Upload vào đúng folder ----
    public async Task<string> UploadFileAsync(IFormFile file, string? folderId = null)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        var drive = await GetDriveServiceAsync();

        var metadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = file.FileName,
            Parents = folderId != null ? new List<string> { folderId } : null
        };

        using var stream = file.OpenReadStream();

        var request = drive.Files.Create(metadata, stream, file.ContentType);
        request.Fields = "id";
        await request.UploadAsync();

        var fileId = request.ResponseBody?.Id;
        if (fileId == null) throw new Exception("Upload failed");

        return $"https://drive.google.com/file/d/{fileId}/view";
    }

    public async Task DeleteFileByIdAsync(string fileId)
    {
        if (string.IsNullOrEmpty(fileId)) return;

        var drive = await GetDriveServiceAsync();
        await drive.Files.Delete(fileId).ExecuteAsync();
    }

    public string ExtractFileIdFromUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return "";
        try
        {
            var start = url.IndexOf("/d/") + 3;
            var end = url.IndexOf('/', start);
            if (end < 0) end = url.Length;
            return url.Substring(start, end - start);
        }
        catch { return ""; }
    }
}
