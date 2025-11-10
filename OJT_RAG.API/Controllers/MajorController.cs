using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MajorController : ControllerBase
    {
        private readonly IMajorService _service;

        public MajorController(IMajorService service)
        {
            _service = service;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllMajors());
        }


        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var major = await _service.GetMajor(id);
            return major == null ? NotFound() : Ok(major);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Major major)
        {
            var result = await _service.CreateMajor(major);
            return CreatedAtAction(nameof(Get), new { id = result.MajorId }, result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] Major major)
        {
            var result = await _service.UpdateMajor(id, major);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            return await _service.DeleteMajor(id) ? Ok("Deleted") : NotFound();
        }
    }
}
aaa