using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using OJT_RAG.Models;
using OJT_RAG.Repositories;
using System.Text.Json;

namespace OJT_RAG.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly Cloudinary _cloudinary;
        private readonly DocumentRepository _repo;

        public DocumentService(Cloudinary cloudinary, DocumentRepository repo)
        {
            _cloudinary = cloudinary;
            _repo = repo;
        }

        public async Task<Document> UploadDocumentAsync(IFormFile file, int uploadedBy)
        {
            // 1️⃣ Upload file lên Cloudinary
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "documents"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // 2️⃣ Đọc nội dung file và convert sang JSON string
            string jsonData;
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension == ".pdf")
            {
                jsonData = PdfConverter.ConvertDocToJson(file);
            }
            else if (extension == ".docx")
            {
                jsonData = DocTextExtractor.ExtractTextToJson(file);
            }
            else
            {
                throw new NotSupportedException("Chỉ hỗ trợ file PDF hoặc DOCX");
            }

            // 3️⃣ Tạo Document entity
            var doc = new Document
            {
                Title = Path.GetFileNameWithoutExtension(file.FileName),
                FilePath = uploadResult.SecureUrl.ToString(),
                UploadedBy = uploadedBy,
                Type = "Policy",
                Status = DocumentStatus.Draft,
                JsonContent = jsonData

            };

            // 4️⃣ Lưu vào database
            return await _repo.AddAsync(doc);
        }
    }
}
