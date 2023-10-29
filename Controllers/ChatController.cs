using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IASquad.Poc.AzureOpenAi.Controllers;

public class ChatController : Controller
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    public IActionResult Index()
    {
        return View();
    }
}
