using Azure;
using Azure.AI.OpenAI;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Services;

public class ImageService : IImageService
{
    private readonly OpenAIClient _openAIClient;

    public ImageService(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

    public async Task<string> GetImage(string prompt, ImageSize size)
    {
        Response<ImageGenerations> imageGenerations = await _openAIClient.GetImageGenerationsAsync(
            new ImageGenerationOptions()
            {
                Prompt = prompt,
                Size = size,
            });

        // Image Generations responses provide URLs you can use to retrieve requested images
        Uri imageUri = imageGenerations.Value.Data[0].Url;

        return imageUri.ToString();
    }
}
