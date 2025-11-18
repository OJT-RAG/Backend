using Microsoft.AspNetCore.Mvc;
using OJT_RAG.DTOs.CompanyDocumentDTO;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/company-document")]
    [ApiController]
    public class CompanyDocumentController : ControllerBase
    {
        private readonly ICompanyDocumentService _service;

        public CompanyDocumentController(ICompanyDocumentService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
            => Ok(await _service.GetById(id));

        [HttpGet("semester/{semId}")]
        public async Task<IActionResult> GetBySemester(long semId)
            => Ok(await _service.GetBySemester(semId));

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateCompanyDocumentDTO dto)
            => Ok(await _service.Create(dto));

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateCompanyDocumentDTO dto)
            => Ok(await _service.Update(dto));

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
            => Ok(await _service.Delete(id));
    }
}
