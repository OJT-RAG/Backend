using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface IOjtDocumentRepository
    {
        Task<IEnumerable<Ojtdocument>> GetAllAsync();
        Task<Ojtdocument?> GetByIdAsync(long id);
        Task<IEnumerable<Ojtdocument>> GetBySemesterAsync(long semesterId);
        Task<IEnumerable<Ojtdocument>> GetByTagTypeAsync(string type, bool includeRelated = true);
        Task<Ojtdocument> AddAsync(Ojtdocument entity);
        Task<Ojtdocument> UpdateAsync(Ojtdocument entity);
        Task<bool> DeleteAsync(long id);
    }

}
