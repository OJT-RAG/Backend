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
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        try
        {
            // Kiểm tra file hợp lệ
            if (file == null)
                return BadRequest("File is required.");

            if (file.Length == 0)
                return BadRequest("Uploaded file is empty.");

            // Thực hiện upload
            var fileId = await _driveService.UploadFileAsync(file);

            // Trả về JSON chuẩn
            return Ok(new { fileId = fileId });
        }
        catch (Exception ex)
        {
            // Trả lỗi rõ ràng
            return StatusCode(500, new
            {
                message = "Upload to Google Drive failed.",
                error = ex.Message
            });
        }
    }
}
