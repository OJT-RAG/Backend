using OJT_RAG.DTOs.CompanyDocumentTag;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class CompanyDocumentTagService : ICompanyDocumentTagService
    {
        private readonly ICompanyDocumentTagRepository _repo;

        public CompanyDocumentTagService(ICompanyDocumentTagRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Companydocumenttag>> GetAll()
            => await _repo.GetAllAsync();

        public async Task<Companydocumenttag?> GetById(long id)
            => await _repo.GetByIdAsync(id);

        public async Task<bool> Create(CreateCompanyDocumentTagDTO dto)
        {
            var entity = new Companydocumenttag
            {
                CompanyDocumentId = dto.CompanyDocumentId,
                DocumentTagId = dto.DocumentTagId
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateCompanyDocumentTagDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.CompanyDocumentTagId);
            if (entity == null) return false;

            entity.CompanyDocumentId = dto.CompanyDocumentId;
            entity.DocumentTagId = dto.DocumentTagId;

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
            => await _repo.DeleteAsync(id);
    }
}
