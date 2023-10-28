using Azure.AI.OpenAI;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;

namespace IASquad.Poc.AzureOpenAi.Services;

public class ChatService : IChatService
{
    private readonly OpenAIClient _openAIClient;

    public ChatService(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }
}
