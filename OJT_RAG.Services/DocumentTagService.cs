using OJT_RAG.DTOs.DocumentTagDTO;
using OJT_RAG.ModelView.DocumentTagModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class DocumentTagService : IDocumentTagService
    {
        private readonly IDocumentTagRepository _repo;

        public DocumentTagService(IDocumentTagRepository repo)
        {
            _repo = repo;
        }

        private DocumentTagModelView Map(Documenttag x)
        {
            return new DocumentTagModelView
            {
                DocumenttagId = x.DocumenttagId,
                Name = x.Name,
                Type = x.Type.ToString()
            };
        }

        public async Task<IEnumerable<DocumentTagModelView>> GetAll()
        {
            return (await _repo.GetAllAsync()).Select(Map);
        }

        public async Task<DocumentTagModelView?> GetById(long id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : Map(x);
        }

        public async Task<bool> Create(CreateDocumentTagDTO dto)
        {
            var entity = new Documenttag
            {
                Name = dto.Name,
                Type = dto.Type
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateDocumentTagDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.DocumenttagId);
            if (entity == null) return false;

            if (!string.IsNullOrEmpty(dto.Name))
                entity.Name = dto.Name;

            if (dto.Type.HasValue) // Kiểm tra nếu có cập nhật Type
                entity.Type = dto.Type.Value;

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }


    }
}
