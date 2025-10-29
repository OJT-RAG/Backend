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

        // ✅ Upload bất kỳ file nào (docx, pdf, txt, ...).
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "documents"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        // ✅ Upload PDF và chuyển trang đầu thành ảnh thumbnail (dùng cho CV/preview)
        public async Task<string> UploadPdfAsImageAsync(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return null;

            // 1️⃣ Lưu tạm PDF
            var tempPdfPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPdfPath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            // 2️⃣ Chuyển trang đầu PDF thành ảnh (sử dụng PdfiumViewer)
            byte[] imageBytes = PdfConverter.ConvertPdfPageToImage(tempPdfPath);

            // 3️⃣ Xóa file PDF tạm
            File.Delete(tempPdfPath);

            // 4️⃣ Upload ảnh lên Cloudinary
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
