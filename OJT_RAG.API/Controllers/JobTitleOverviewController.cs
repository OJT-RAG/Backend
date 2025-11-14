using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitleOverviewController : ControllerBase
    {
        private readonly IJobTitleOverviewService _service;

        public JobTitleOverviewController(IJobTitleOverviewService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetById(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(JobTitleOverview model)
        {
            return Ok(await _service.Create(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(JobTitleOverview model)
        {
            return Ok(await _service.Update(model));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
