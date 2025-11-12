using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllCompanies());

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(long id)
            => Ok(await _service.GetCompany(id));

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Company dto)
        {
            await _service.CreateCompany(dto);
            return Ok("Created successfully");
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Company dto)
        {
            await _service.UpdateCompany(id, dto);
            return Ok("Updated successfully");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteCompany(id);
            return Ok("Deleted successfully");
        }
    }

}
