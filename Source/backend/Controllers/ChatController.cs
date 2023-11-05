using IASquad.Poc.AzureOpenAi.Models.Chat;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Controllers;

[ApiController]
[Route("chat")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("ask")]
    public async Task<ActionResult<string>> AskAsync([FromBody] ChatAskViewModel model)
    {
        var response = await _chatService.GetChatCompletionAsync(model.SystemPrompts, model.Question);

        return Ok(response);
    }
}
