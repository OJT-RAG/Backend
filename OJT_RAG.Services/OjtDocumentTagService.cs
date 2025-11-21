using OJT_RAG.DTOs.OjtDocumentTag;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class OjtDocumentTagService : IOjtDocumentTagService
    {
        private readonly IOjtDocumentTagRepository _repo;

        public OjtDocumentTagService(IOjtDocumentTagRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Ojtdocumenttag>> GetAll()
            => await _repo.GetAllAsync();

        public async Task<Ojtdocumenttag?> GetById(long id)
            => await _repo.GetByIdAsync(id);

        public async Task<bool> Create(CreateOjtDocumentTagDTO dto)
        {
            var entity = new Ojtdocumenttag
            {
                OjtDocumentId = dto.OjtDocumentId,
                DocumentTagId = dto.DocumentTagId
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateOjtDocumentTagDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.OjtDocumentTagId);
            if (entity == null) return false;

            entity.OjtDocumentId = dto.OjtDocumentId;
            entity.DocumentTagId = dto.DocumentTagId;

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
            => await _repo.DeleteAsync(id);
    }
}
