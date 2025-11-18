using Microsoft.AspNetCore.Mvc;

[Route("api/files")]
[ApiController]
public class FileUploadController : ControllerBase
{
    private readonly GoogleDriveService _driveService;

    public FileUploadController(GoogleDriveService driveService)
    {
        _driveService = driveService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var fileId = await _driveService.UploadFileAsync(file);
        return Ok(fileId);
    }
}
