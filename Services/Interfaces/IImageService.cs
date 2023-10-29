using Azure.AI.OpenAI;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Services.Interfaces;

public interface IImageService
{
    Task<string> GetImageAsync(string prompt, ImageSize size);
}
