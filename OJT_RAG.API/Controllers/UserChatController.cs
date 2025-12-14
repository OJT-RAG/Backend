using Microsoft.AspNetCore.Mvc;
using OJT_RAG.Repositories.Interfaces;

[ApiController]
[Route("api/user-chat")]
public class UserChatController : ControllerBase
{
    private readonly IUserChatRepository _repo;

    public UserChatController(IUserChatRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("conversation")]
    public async Task<IActionResult> Get(long user1, long user2)
    {
        var data = await _repo.GetConversation(user1, user2);
        return Ok(data);
    }
}
