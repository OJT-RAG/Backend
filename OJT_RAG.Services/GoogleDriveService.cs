using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class GoogleDriveService
{
    public readonly DriveService _drive;

    public GoogleDriveService(IConfiguration config)
    {
        var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
        var refreshToken = Environment.GetEnvironmentVariable("GOOGLE_REFRESH_TOKEN");

        if (string.IsNullOrEmpty(clientId) ||
            string.IsNullOrEmpty(clientSecret) ||
            string.IsNullOrEmpty(refreshToken))
        {
            throw new Exception("Google OAuth environment variables are missing");
        }

        var json = $@"{{
            ""type"": ""authorized_user"",
            ""client_id"": ""{clientId}"",
            ""client_secret"": ""{clientSecret}"",
            ""refresh_token"": ""{refreshToken}""
        }}";

        var credential = GoogleCredential
            .FromJson(json)
            .CreateScoped(DriveService.Scope.DriveFile);

        _drive = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = config["Google:ApplicationName"] ?? "OJT_RAG"
        });
    }

    // ================= FOLDER =================
    public async Task<string> GetOrCreateFolderAsync(string folderName, string? parentId = null)
    {
        string q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName.Replace("'", "\\'")}' and trashed=false";
        if (parentId != null)
            q += $" and '{parentId}' in parents";

        var listReq = _drive.Files.List();
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

        var createReq = _drive.Files.Create(metadata);
        createReq.Fields = "id";
        var created = await createReq.ExecuteAsync();

        return created.Id;
    }

    // ================= UPLOAD =================
    public async Task<string> UploadFileAsync(IFormFile file, string? folderId = null)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        const long MAX_SIZE = 50 * 1024 * 1024;
        if (file.Length > MAX_SIZE)
            throw new ArgumentException("File exceeds 50MB");

        var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".png", ".jpg", ".jpeg" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            throw new ArgumentException("Invalid file type");

        var metadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = file.FileName,
            Parents = folderId != null ? new List<string> { folderId } : null
        };

        using var stream = file.OpenReadStream();

        var request = _drive.Files.Create(metadata, stream, file.ContentType);
        request.Fields = "id";
        await request.UploadAsync();

        return $"https://drive.google.com/file/d/{request.ResponseBody.Id}/view";
    }

    // ================= DELETE =================
    public async Task DeleteFileByIdAsync(string fileId)
    {
        if (!string.IsNullOrEmpty(fileId))
            await _drive.Files.Delete(fileId).ExecuteAsync();
    }

    public string ExtractFileIdFromUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return "";
        var start = url.IndexOf("/d/") + 3;
        var end = url.IndexOf('/', start);
        if (end < 0) end = url.Length;
        return url[start..end];
    }


    // ================= DOWNLOAD =================
    public async Task<(byte[] fileBytes, string fileName, string contentType)> DownloadFileByUrlAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            throw new ArgumentException("FileUrl is empty");

        var fileId = ExtractFileIdFromUrl(fileUrl);
        if (string.IsNullOrEmpty(fileId))
            throw new Exception("Invalid Google Drive file URL");

        // 🔹 Lấy metadata
        var meta = await _drive.Files.Get(fileId).ExecuteAsync();

        var fileName = meta.Name;
        var contentType = meta.MimeType ?? "application/octet-stream";

        // 🔹 Download file
        using var stream = new MemoryStream();
        await _drive.Files.Get(fileId).DownloadAsync(stream);

        return (stream.ToArray(), fileName, contentType);
    }

}
