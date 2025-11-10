using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Repositories.Context;

namespace OJT_RAG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseTestController : ControllerBase
    {
        private readonly OJTRAGContext _context;

        public DatabaseTestController(OJTRAGContext context)
        {
            _context = context;
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                return Ok(new { connected = canConnect });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
