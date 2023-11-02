using System.Collections.Generic;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Services.Interfaces;

public interface IChatService
{
    Task<string> GetChatCompletionAsync(IEnumerable<string> systemPrompts, string userPrompt);
}
