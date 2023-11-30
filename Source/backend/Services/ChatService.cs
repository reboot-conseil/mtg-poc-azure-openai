using Azure.AI.OpenAI;
using IASquad.Poc.AzureOpenAi.Data.Entities;
using IASquad.Poc.AzureOpenAi.Data.Functions;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

    public async Task<string> GetChatCompletionWithFunctionCallAsync(IEnumerable<string> systemPrompts, string userPrompt)
    {
        var chatMessages = systemPrompts.Select(sp => new ChatMessage(ChatRole.System, sp)).ToList();


        ChatCompletions response;
        ChatChoice responseChoice;

        // Ajoute les fonctions supplémentaires au prompt
        FunctionDefinition getWeatherFuntionDefinition = GetWeatherFunction.GetFunctionDefinition();
        chatMessages.Add(new ChatMessage(ChatRole.User, userPrompt));
        ChatCompletionsOptions chatCompletionsOptions = new(chatMessages)
        {
            Temperature = (float)0.7,
            MaxTokens = 1000,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
        };
        chatCompletionsOptions.Functions.Add(getWeatherFuntionDefinition);

        // On appelle l'API d'Azure OpenAI avec le prompt
        response =
                await _openAIClient.GetChatCompletionsAsync(
                    "gpt-35-turbo-16k",
                    chatCompletionsOptions);

        responseChoice = response.Choices[0];

        while (responseChoice.FinishReason == CompletionsFinishReason.FunctionCall)
        {
            // Tant qu'on detecte un function call, on l'ajoute
            chatCompletionsOptions.Messages.Add(responseChoice.Message);

            if (responseChoice.Message.FunctionCall.Name == GetWeatherFunction.Name)
            {
                string parametres = responseChoice.Message.FunctionCall.Arguments;

                WeatherInput input = JsonSerializer.Deserialize<WeatherInput>(parametres,
                        new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                //On appelle la fonction qu'on souhaite appelé pour récupérer la donnée
                var resultatFonction = GetWeatherFunction.GetWeather(input.Location, input.Unit);

                // On ajoute la réponse à la conversation
                var messageReponse = new ChatMessage(
                    ChatRole.Function,
                    JsonSerializer.Serialize(
                        resultatFonction,
                        new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                {
                    Name = GetWeatherFunction.Name
                };
                chatCompletionsOptions.Messages.Add(messageReponse);
            }

            // On rappelle l'API pour avoir la réponse de GPT
            response = await _openAIClient.GetChatCompletionsAsync(
                    "gpt-35-turbo-16k",
                    chatCompletionsOptions);

            responseChoice = response.Choices[0];
        }

        return responseChoice.Message.Content;
    }

    public async Task<string> GetChatCompletionWithContextAsync(IEnumerable<Message> messages)
    {
        // Conversion des messages en Messages pour GPT
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
        // Résume une conversation pour rester dans la limite de 160000 tokens (input + completions)

        // Nos prompts système pour résumer la conversation
        List<ChatMessage> systemPrompts = new()
        {
             new ChatMessage(ChatRole.System, "Résume la conversation suivante en un court paragraphe d'environ 150 mots."),
             new ChatMessage(ChatRole.System, "la conversation est délimitée par ```"),
        };

        // Ajoute les délimiteurs de la conversation
        var chatMessages = messages.Where(m => m.Role != MessageRole.SYSTEM)
            .Select(ConvertMessageToChatMessage)
            .ToList();

        chatMessages[0].Content = "```" + chatMessages[0].Content;
        chatMessages.Last().Content = chatMessages.Last().Content + "```";

        chatMessages = systemPrompts.Concat(chatMessages).ToList();
        // On demande à GPT de nous résumer la conversation
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
        // Converti mes messages en messages pour le SDK
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
