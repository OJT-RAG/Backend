using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OJT_RAG.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repo;

        public CompanyService(ICompanyRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Company>> GetAllCompanies()
            => _repo.GetAll();

        public Task<Company?> GetCompany(long id)
            => _repo.GetById(id);

        public Task CreateCompany(Company dto)
            => _repo.Add(dto);

        public async Task UpdateCompany(long id, Company dto)
        {
            dto.CompanyId = id;
            await _repo.Update(dto);
        }

        public Task DeleteCompany(long id)
            => _repo.Delete(id);
    }

}
