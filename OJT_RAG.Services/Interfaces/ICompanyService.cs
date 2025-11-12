using OJT_RAG.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OJT_RAG.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAllCompanies();
        Task<Company?> GetCompany(long id);
        Task CreateCompany(Company dto);
        Task UpdateCompany(long id, Company dto);
        Task DeleteCompany(long id);
    }
}
