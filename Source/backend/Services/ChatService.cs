using Azure.AI.OpenAI;
using IASquad.Poc.AzureOpenAi.Data.Entities;
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
        var response = await _openAIClient.GetChatCompletionsAsync("chat-test",
            new ChatCompletionsOptions(chatMessages)
            {
                Temperature = (float)0.7,
                MaxTokens = 1000,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            });

        var completions = response.Value;

        return completions.Choices[0].Message.Content;
    }

    public async Task<string> GetChatCompletionWithContextAsync(IEnumerable<Message> messages)
    {
        var chatMessages = messages.Select(ConvertMessageToChatMessage).ToList();
        var response = await _openAIClient.GetChatCompletionsAsync("chat-test",
            new ChatCompletionsOptions(chatMessages)
            {
                Temperature = (float)0.7,
                MaxTokens = 10000,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            });

        var completions = response.Value;

        return completions.Choices[0].Message.Content;
    }

    public async Task<string> SummarizeContextAsync(IEnumerable<Message> messages)
    {
        List<ChatMessage> systemPrompts = new()
        {
             new ChatMessage(ChatRole.System, "Résume la conversation suivante en un court paragraphe d'environ 150 mots."),
             new ChatMessage(ChatRole.System, "la conversation est délimitée par ```"),
        };
        var chatMessages = messages.Where(m => m.Role != MessageRole.SYSTEM)
            .Select(ConvertMessageToChatMessage)
            .ToList();

        chatMessages[0].Content = "```" + chatMessages[0].Content;
        chatMessages.Last().Content = chatMessages[0].Content + "```";

        chatMessages = systemPrompts.Concat(chatMessages).ToList();
        var response = await _openAIClient.GetChatCompletionsAsync("chat-test",
            new ChatCompletionsOptions(chatMessages)
            {
                Temperature = (float)0.7,
                MaxTokens = 1000,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            });

        var completions = response.Value;

        return completions.Choices[0].Message.Content;
    }

    private ChatMessage ConvertMessageToChatMessage(Message message)
    {
        var role = message.Role switch
        {
            MessageRole.SYSTEM => ChatRole.System,
            MessageRole.ASSISTANT => ChatRole.Assistant,
            MessageRole.USER => ChatRole.User,
            _ => ChatRole.User,
        };
        return new ChatMessage(role, message.Value);
    }
}
