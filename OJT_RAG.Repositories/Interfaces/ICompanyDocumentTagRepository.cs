using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.Repositories.Interfaces
{
    public interface ICompanyDocumentTagRepository
    {
        Task<bool> ExistsAsync(long companyDocumentId, long documentTagId);
        Task<IEnumerable<Documenttag>> GetTagsByDocumentId(long ojtDocumentId);
        Task AddAsync(Companydocumenttag entity);
        Task RemoveAsync(long companyDocumentId, long documentTagId);
    }
}
