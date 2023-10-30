using Azure;
using Azure.AI.OpenAI;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Services;

public class ChatService : IChatService
{
    private readonly OpenAIClient _openAIClient;

    public ChatService(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

    public async Task<string> GetChatCompletionAsync(IEnumerable<string> systemPrompts, string userPrompt)
    {
        var chatMessages = systemPrompts.Select(sp => new ChatMessage(ChatRole.System, sp)).ToList();
        chatMessages.Add(new ChatMessage(ChatRole.User, userPrompt));
        Response<ChatCompletions> responseWithoutStream = await _openAIClient.GetChatCompletionsAsync("chat-test",
            new ChatCompletionsOptions(chatMessages)
            {
                Temperature = (float)0.7,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            });

        ChatCompletions completions = responseWithoutStream.Value;

        return completions.Choices[0].Message.Content;
    }
}
