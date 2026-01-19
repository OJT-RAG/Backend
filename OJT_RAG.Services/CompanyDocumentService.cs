using System.Net.Http;
using OJT_RAG.DTOs.CompanyDocumentDTO;
using OJT_RAG.ModelViews.CompanyDocument;
using OJT_RAG.Repositories;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;
namespace OJT_RAG.Services
{
    public class CompanyDocumentService : ICompanyDocumentService
    {
        private readonly ICompanyDocumentRepository _repo;
        private readonly GoogleDriveService _drive;
        private readonly ICompanyDocumentTagRepository _tagRepo;
        public CompanyDocumentService(ICompanyDocumentRepository repo, GoogleDriveService drive, ICompanyDocumentTagRepository tagRepo)
        {
            _repo = repo;
            _drive = drive;
            _tagRepo = tagRepo;
        }

        private CompanyDocumentModelView Map(Companydocument x)
        {
            return new CompanyDocumentModelView
            {
                CompanyDocumentId = x.CompanydocumentId,
                SemesterCompanyId = x.SemesterCompanyId,
                Title = x.Title,
                FileUrl = x.FileUrl,
                UploadedBy = x.UploadedBy,
                IsPublic = x.IsPublic
            };
        }

        public async Task<IEnumerable<CompanyDocumentModelView>> GetAll()
            => (await _repo.GetAllAsync()).Select(Map);

        public async Task<CompanyDocumentModelView?> GetById(long id)
            => (await _repo.GetByIdAsync(id)) is Companydocument x ? Map(x) : null;

        public async Task<IEnumerable<CompanyDocumentModelView>> GetBySemester(long semId)
            => (await _repo.GetBySemesterCompanyIdAsync(semId)).Select(Map);

        public async Task<bool> Create(CreateCompanyDocumentDTO dto)
        {
            try
            {
                // 1. Quản lý thư mục Google Drive
                var rootFolderId = await _drive.GetOrCreateFolderAsync("OJT_RAG");
                var folderName = $"Company_Document_Semester_{dto.SemesterCompanyId}";
                var childFolderId = await _drive.GetOrCreateFolderAsync(folderName, rootFolderId);

                // 2. Xử lý Upload file
                string? fileUrl = null;
                if (dto.File != null && dto.File.Length > 0)
                {
                    fileUrl = await _drive.UploadFileAsync(dto.File, childFolderId);
                }

                // 3. Khởi tạo Entity (Dựa trên DTO giữ nguyên của bạn)
                var entity = new Companydocument
                {
                    SemesterCompanyId = dto.SemesterCompanyId,
                    Title = !string.IsNullOrWhiteSpace(dto.Title) ? dto.Title : (dto.File?.FileName ?? "Tài liệu không tên"),
                    UploadedBy = dto.UploadedBy ?? 0,
                    IsPublic = dto.IsPublic ?? true,
                    FileUrl = fileUrl
                };

                // 4. THỰC HIỆN LƯU (SỬA LỖI TẠI ĐÂY)
                await _repo.AddAsync(entity);

                // Nếu dòng trên chạy không lỗi, trả về true
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Create Error]: {ex.Message}");
                throw; // Ném lỗi để Controller xử lý thông báo 400/500
            }
        }


        public async Task<bool> Update(UpdateCompanyDocumentDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.CompanyDocumentId);
            if (entity == null) return false;

            entity.SemesterCompanyId = dto.SemesterCompanyId;
            entity.Title = dto.Title;
            entity.UploadedBy = dto.UploadedBy;
            entity.IsPublic = dto.IsPublic;

            if (dto.File != null)
            {
                if (!string.IsNullOrEmpty(entity.FileUrl))
                {
                    var oldId = _drive.ExtractFileIdFromUrl(entity.FileUrl);
                    if (!string.IsNullOrEmpty(oldId))
                    {
                        try
                        {
                            await _drive.DeleteFileByIdAsync(oldId);
                        }
                        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            // File đã không còn trên Drive → bỏ qua
                            Console.WriteLine($"File {oldId} không tồn tại, bỏ qua xóa.");
                        }
                    }
                }

                var folderName = $"Company_Document_Semester_{dto.SemesterCompanyId}";
                var folderId = await _drive.GetOrCreateFolderAsync(folderName);

                entity.FileUrl = await _drive.UploadFileAsync(dto.File, folderId);
            }

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

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
                        // File đã không còn trên Drive → bỏ qua
                        Console.WriteLine($"File {fileId} không tồn tại, bỏ qua xóa.");
                    }
                }
            }

            return await _repo.DeleteAsync(id);
        }

        public async Task<(byte[] fileBytes, string fileName, string contentType)?> Download(long id)
        {
            var doc = await _repo.GetByIdAsync(id);
            if (doc == null || string.IsNullOrEmpty(doc.FileUrl))
                return null;

            return await _drive.DownloadFileByUrlAsync(doc.FileUrl);
        }
        public async Task<IEnumerable<Documenttag>> GetTags(long companyDocumentId)
        {
            return await _tagRepo.GetTagsByDocumentId(companyDocumentId);
        }

        public async Task AddTag(long documentId, long tagId)
        {
            if (!await _tagRepo.ExistsAsync(documentId, tagId))
            {
                await _tagRepo.AddAsync(new Companydocumenttag
                {
                    CompanyDocumentId = documentId,
                    DocumentTagId = tagId
                });
            }
        }

        public async Task RemoveTag(long documentId, long tagId)
        {
            await _tagRepo.RemoveAsync(documentId, tagId);
        }
    }
}
