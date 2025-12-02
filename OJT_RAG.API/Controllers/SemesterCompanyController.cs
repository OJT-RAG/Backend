using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.SemesterCompanyDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/semester-company")]
    [ApiController]
    public class SemesterCompanyController : ControllerBase
    {
        private readonly ISemesterCompanyService _service;

        public SemesterCompanyController(ISemesterCompanyService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
            => Ok(await _service.GetById(id));

        [HttpGet("semester/{semesterId}")]
        public async Task<IActionResult> GetBySemester(long semesterId)
            => Ok(await _service.GetBySemester(semesterId));

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(long companyId)
            => Ok(await _service.GetByCompany(companyId));

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateSemesterCompanyDTO dto)
            => Ok(await _service.Create(dto));

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateSemesterCompanyDTO dto)
            => Ok(await _service.Update(dto));

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
            => Ok(await _service.Delete(id));
    }
}
