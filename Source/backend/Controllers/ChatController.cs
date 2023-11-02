using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
}
