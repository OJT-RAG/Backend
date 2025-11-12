using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Services.Interfaces;
using OJT_RAG.Repositories.Entities;

namespace OJT_RAG.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobDescriptionController : ControllerBase
    {
        private readonly IJobDescriptionService _service;

        public JobDescriptionController(IJobDescriptionService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(JobDescription model)
        {
            return Ok(await _service.Create(model));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(JobDescription model)
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
