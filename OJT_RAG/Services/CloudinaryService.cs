using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace OJT_RAG.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var cloudName = "dnis5z1ax";
            var apiKey = "258332365461753";
            var apiSecret = "ysTEGF2bcEy_uogbVgaL43m0Vhs";

            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
        }

        public async Task<string> UploadPdfAsImageAsync(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return null;

            // 1. Lưu tạm PDF
            var tempPdfPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPdfPath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            // 2. Chuyển trang đầu PDF thành ảnh
            byte[] imageBytes = PdfConverter.ConvertPdfPageToImage(tempPdfPath);

            // 3. Xóa PDF tạm
            File.Delete(tempPdfPath);

            // 4. Upload ảnh lên Cloudinary
            using var imgStream = new MemoryStream(imageBytes);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(pdfFile.FileName.Replace(".pdf", ".jpg"), imgStream),
                Folder = "cv_images"
            };

            var uploadResult = _cloudinary.Upload(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
