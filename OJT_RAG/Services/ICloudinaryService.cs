using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public interface ICloudinaryService
{
    /// <summary>
    /// Upload PDF → chuyển trang đầu thành ảnh → upload ảnh lên Cloudinary
    /// </summary>
    Task<string> UploadPdfAsImageAsync(IFormFile pdfFile);
}
