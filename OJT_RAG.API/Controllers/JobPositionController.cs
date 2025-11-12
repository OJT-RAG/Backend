using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPositionController : ControllerBase
    {
        private readonly IJobPositionService _service;

        public JobPositionController(IJobPositionService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] JobPosition dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.JobPositionId }, created);
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] JobPosition dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
            => Ok(await _service.DeleteAsync(id));
    }
}
