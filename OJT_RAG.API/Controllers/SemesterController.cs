using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemesterController(ISemesterService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var r = await _service.GetByIdAsync(id);
            return r == null ? NotFound() : Ok(r);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Semester dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.SemesterId }, created);
        }

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] Semester dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> Delete(long id) => Ok(await _service.DeleteAsync(id));
    }
}
