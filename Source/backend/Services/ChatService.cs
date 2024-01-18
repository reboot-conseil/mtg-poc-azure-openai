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
        List<ChatRequestMessage> chatMessages = systemPrompts.Select(sp => new ChatRequestSystemMessage(sp) as ChatRequestMessage).ToList();
        chatMessages.Add(new ChatRequestUserMessage(userPrompt));
        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions("chat-test", chatMessages)
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
        var chatMessages = systemPrompts.Select(sp => new ChatRequestSystemMessage(sp) as ChatRequestMessage).ToList();


        ChatCompletions response;
        ChatChoice responseChoice;

        // Ajoute les fonctions supplémentaires au prompt
        FunctionDefinition getWeatherFuntionDefinition = GetWeatherFunction.GetFunctionDefinition();
        chatMessages.Add(new ChatRequestUserMessage(userPrompt));
        var chatCompletionsOptions = new ChatCompletionsOptions("gpt-4-32k", chatMessages)
        {
            Temperature = (float)0.7,
            MaxTokens = 1000,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
        };
        chatCompletionsOptions.Functions.Add(getWeatherFuntionDefinition);

        // On appelle l'API d'Azure OpenAI avec le prompt
        response =
                await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);

        responseChoice = response.Choices[0];

        while (responseChoice.FinishReason == CompletionsFinishReason.FunctionCall)
        {
            // Tant qu'on detecte un function call, on l'ajoute
            chatCompletionsOptions.Messages.Add(new ChatRequestAssistantMessage(responseChoice.Message.Content)
            {
                FunctionCall = responseChoice.Message.FunctionCall,
            });

            if (responseChoice.Message.FunctionCall.Name == GetWeatherFunction.Name)
            {
                string parametres = responseChoice.Message.FunctionCall.Arguments;

                WeatherInput input = JsonSerializer.Deserialize<WeatherInput>(parametres,
                        new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                //On appelle la fonction qu'on souhaite appelé pour récupérer la donnée
                var resultatFonction = GetWeatherFunction.GetWeather(input.Location, input.Unit);

                // On ajoute la réponse à la conversation
                var functionResponseMessage = new ChatRequestFunctionMessage(
                    name: responseChoice.Message.FunctionCall.Name,
                    content: JsonSerializer.Serialize(
                        resultatFonction,
                        new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    )
                );

                chatCompletionsOptions.Messages.Add(functionResponseMessage);
            }

            // On rappelle l'API pour avoir la réponse de GPT
            response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);

            responseChoice = response.Choices[0];
        }

        return responseChoice.Message.Content;
    }

    public async Task<string> GetChatCompletionWithContextAsync(IEnumerable<Message> messages)
    {
        // Conversion des messages en Messages pour GPT
        var chatMessages = messages.Select(ConvertMessageToChatMessage).ToList();

        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions("chat-test", chatMessages)
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
        List<ChatRequestMessage> systemPrompts = new()
        {
            new ChatRequestSystemMessage("Résume la conversation suivante en un court paragraphe d'environ 150 mots."),
            new ChatRequestSystemMessage("la conversation est délimitée par ```"),
        };

        // Ajoute les délimiteurs de la conversation
        messages.First().Value = "```" + messages.First().Value;
        messages.Last().Value = messages.Last().Value + "```";

        var chatMessages = messages.Where(m => m.Role != MessageRole.SYSTEM)
            .Select(ConvertMessageToChatMessage)
            .ToList();

        chatMessages = systemPrompts.Concat(chatMessages).ToList();
        // On demande à GPT de nous résumer la conversation
        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions("chat-test", chatMessages)
            {
                Temperature = (float)0.7,
                MaxTokens = 1000,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            });

        var completions = response.Value;

        return completions.Choices[0].Message.Content;
    }

    private ChatRequestMessage ConvertMessageToChatMessage(Message message)
    {
        // Converti mes messages en messages pour le SDK
        return message.Role switch
        {
            MessageRole.SYSTEM => new ChatRequestSystemMessage(message.Value),
            MessageRole.ASSISTANT => new ChatRequestAssistantMessage(message.Value),
            MessageRole.USER => new ChatRequestUserMessage(message.Value),
            _ => new ChatRequestUserMessage(message.Value),
        };
    }
}
