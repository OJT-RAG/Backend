using OJT_RAG.DTOs.CompanyDocumentDTO;
using OJT_RAG.ModelViews.CompanyDocument;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class CompanyDocumentService : ICompanyDocumentService
    {
        private readonly ICompanyDocumentRepository _repo;
        private readonly GoogleDriveService _drive;

        public CompanyDocumentService(ICompanyDocumentRepository repo, GoogleDriveService drive)
        {
            _repo = repo;
            _drive = drive;
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
            // 1. Folder cha OJT_RAG
            var rootFolderId = await _drive.GetOrCreateFolderAsync("OJT_RAG");

            // 2. Folder con theo SemesterCompanyId
            var folderName = $"semester_company_{dto.SemesterCompanyId}";
            var childFolderId = await _drive.GetOrCreateFolderAsync(folderName, rootFolderId);

            // 3. Upload file vào folder con
            string? fileUrl = null;
            if (dto.File != null)
                fileUrl = await _drive.UploadFileAsync(dto.File, childFolderId);

            // 4. Save DB
            var entity = new Companydocument
            {
                SemesterCompanyId = dto.SemesterCompanyId,
                Title = dto.Title,
                UploadedBy = dto.UploadedBy,
                IsPublic = dto.IsPublic,
                FileUrl = fileUrl
            };

            await _repo.AddAsync(entity);
            return true;
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
                        await _drive.DeleteFileByIdAsync(oldId);
                }

                var folderName = $"semester_company_{dto.SemesterCompanyId}";
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
                    await _drive.DeleteFileByIdAsync(fileId);
            }

            return await _repo.DeleteAsync(id);
        }
    }
}
