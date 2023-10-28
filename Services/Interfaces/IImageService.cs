using Azure.AI.OpenAI;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Services.Interfaces;

public interface IImageService
{
    Task<string> GetImage(string prompt, ImageSize size);
}
