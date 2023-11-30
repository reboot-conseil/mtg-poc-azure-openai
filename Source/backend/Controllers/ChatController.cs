using Azure;
using IASquad.Poc.AzureOpenAi.Data.Entities;
using IASquad.Poc.AzureOpenAi.Models.Chat;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

    [HttpPost("ask-function-call")]
    public async Task<ActionResult<string>> AskFunctionCallAsync([FromBody] ChatAskViewModel model)
    {
        var response = await _chatService.GetChatCompletionWithFunctionCallAsync(model.SystemPrompts, model.Question);

        return Ok(response);
    }

    [HttpPost("ask-context")]
    public async Task<ActionResult<ChatContextResponseViewModel>> AskContextAsync([FromBody] ChatContextViewModel model)
    {
        var question = model.Messages.Last();
        try
        {
            // On essaie de répondre
            var response = await _chatService.GetChatCompletionWithContextAsync(model.Messages);
            model.Messages.Add(new Message
            {
                Role = MessageRole.ASSISTANT,
                Value = response,
            });
        } catch(RequestFailedException e)
        {
            // Si on a une erreur car l'input + complétions dépasse le nombre max de tokens, on résume la conversation

            var summary = await _chatService.SummarizeContextAsync(model.Messages);
            // Prompt système car c'est du contexte !
            model.Messages.Add(new Message
            {
                Role = MessageRole.SYSTEM,
                Value = summary,
            });

            model.Messages = model.Messages.Where(m => m.Role == MessageRole.SYSTEM).ToList();
            model.Messages.Add(question);
            // Et on repart du résumé
            var response = await _chatService.GetChatCompletionWithContextAsync(model.Messages);
            model.Messages.Add(new Message
            {
                Role = MessageRole.ASSISTANT,
                Value = response,
            });
        }

        ChatContextResponseViewModel result = new()
        {
            SystemPrompts = model.Messages.Where(m => m.Role == MessageRole.SYSTEM).Select(m => m.Value).ToList(),
            Messages = model.Messages.Where(m => m.Role != MessageRole.SYSTEM).ToList(),
        };

        return Ok(result);
    }
}
