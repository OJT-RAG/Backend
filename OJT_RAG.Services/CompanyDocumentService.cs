using OJT_RAG.DTOs.CompanyDocumentDTO;
using OJT_RAG.ModelView.CompanyDocumentModelView;
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
        {
            return (await _repo.GetAllAsync()).Select(Map);
        }

        public async Task<CompanyDocumentModelView?> GetById(long id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : Map(x);
        }

        public async Task<IEnumerable<CompanyDocumentModelView>> GetBySemester(long semId)
        {
            return (await _repo.GetBySemesterCompanyIdAsync(semId)).Select(Map);
        }

        public async Task<bool> Create(CreateCompanyDocumentDTO dto)
        {
            string? fileUrl = null;

            if (dto.File != null)
            {
                fileUrl = await _drive.UploadFileAsync(dto.File);
            }

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
                entity.FileUrl = await _drive.UploadFileAsync(dto.File);
            }

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
