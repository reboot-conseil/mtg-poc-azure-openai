using IASquad.Poc.AzureOpenAi.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Services.Interfaces;

public interface IChatService
{
    Task<string> GetChatCompletionAsync(IEnumerable<string> systemPrompts, string userPrompt);

    Task<string> GetChatCompletionWithContextAsync(IEnumerable<Message> messages);

    Task<string> SummarizeContextAsync(IEnumerable<Message> messages);

    Task<string> GetChatCompletionWithFunctionCallAsync(IEnumerable<string> systemPrompts, string userPrompt);
}
