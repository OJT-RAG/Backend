using OJT_RAG.DTOs.OjtDocumentDTO;
using OJT_RAG.ModelView.OjtDocumentModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class OjtDocumentService : IOjtDocumentService
    {
        private readonly IOjtDocumentRepository _repo;
        private readonly GoogleDriveService _drive;

        public OjtDocumentService(IOjtDocumentRepository repo, GoogleDriveService drive)
        {
            _repo = repo;
            _drive = drive;
        }

        private OjtDocumentModelView Map(Ojtdocument x)
        {
            return new OjtDocumentModelView
            {
                OjtdocumentId = x.OjtdocumentId,
                Title = x.Title,
                FileUrl = x.FileUrl,
                SemesterId = x.SemesterId,
                IsGeneral = x.IsGeneral,
                UploadedBy = x.UploadedBy,
                UploadedAt = x.UploadedAt
            };
        }

        public async Task<IEnumerable<OjtDocumentModelView>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(Map);

        public async Task<OjtDocumentModelView?> GetByIdAsync(long id)
            => (await _repo.GetByIdAsync(id)) is Ojtdocument x ? Map(x) : null;


        public async Task<OjtDocumentModelView> CreateAsync(CreateOjtDocumentDTO dto)
        {
            // 1. Folder cha OJT_RAG
            var rootFolderId = await _drive.GetOrCreateFolderAsync("OJT_RAG");

            // 2. Folder con theo semester
            var folderName = $"OJT_Document_Semester_{dto.SemesterId}";
            var folderId = await _drive.GetOrCreateFolderAsync(folderName, rootFolderId);

            // 3. Upload file lên Google Drive
            string? fileUrl = null;
            if (dto.File != null)
                fileUrl = await _drive.UploadFileAsync(dto.File, folderId);

            // 4. Lưu DB
            var entity = new Ojtdocument
            {
                Title = dto.Title,
                SemesterId = dto.SemesterId,
                IsGeneral = dto.IsGeneral,
                UploadedBy = dto.UploadedBy,
                FileUrl = fileUrl,
                UploadedAt = DateTime.Now
            };

            await _repo.AddAsync(entity);
            return Map(entity);
        }


        public async Task<OjtDocumentModelView> UpdateAsync(UpdateOjtDocumentDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.OjtdocumentId);
            if (entity == null) throw new Exception("Không tìm thấy tài liệu.");

            entity.Title = dto.Title ?? entity.Title;
            entity.SemesterId = dto.SemesterId ?? entity.SemesterId;
            entity.IsGeneral = dto.IsGeneral ?? entity.IsGeneral;

            if (dto.File != null)
            {
                // Xóa file cũ trên Drive, bắt lỗi nếu không tìm thấy
                if (!string.IsNullOrEmpty(entity.FileUrl))
                {
                    var oldFileId = _drive.ExtractFileIdFromUrl(entity.FileUrl);
                    if (!string.IsNullOrEmpty(oldFileId))
                    {
                        try
                        {
                            await _drive.DeleteFileByIdAsync(oldFileId);
                        }
                        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            // File đã bị xóa → bỏ qua
                            Console.WriteLine($"File {oldFileId} không tồn tại, bỏ qua xóa.");
                        }
                    }
                }

                // Upload file mới
                var folderName = $"OJT_Document_Semester_{entity.SemesterId}";
                var folderId = await _drive.GetOrCreateFolderAsync(folderName);

                entity.FileUrl = await _drive.UploadFileAsync(dto.File, folderId);
            }

            await _repo.UpdateAsync(entity);
            return Map(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            // Xóa file trên Drive, bắt lỗi nếu không tìm thấy
            if (!string.IsNullOrEmpty(entity.FileUrl))
            {
                var fileId = _drive.ExtractFileIdFromUrl(entity.FileUrl);
                if (!string.IsNullOrEmpty(fileId))
                {
                    try
                    {
                        await _drive.DeleteFileByIdAsync(fileId);
                    }
                    catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        // File đã bị xóa → bỏ qua
                        Console.WriteLine($"File {fileId} không tồn tại, bỏ qua xóa.");
                    }
                }
            }

            return await _repo.DeleteAsync(id);
        }
        public async Task<(byte[] fileBytes, string fileName, string contentType)?> DownloadAsync(long id)
        {
            var doc = await _repo.GetByIdAsync(id);
            if (doc == null || string.IsNullOrEmpty(doc.FileUrl))
                return null;

            return await _drive.DownloadFileByUrlAsync(doc.FileUrl);
        }


    }
}
