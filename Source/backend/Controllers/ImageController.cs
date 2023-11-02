using Azure.AI.OpenAI;
using IASquad.Poc.AzureOpenAi.Models.Image;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{

    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] ImageGenerateViewModel model)
    {
        ImageSize imageSize;
        switch(model.Size)
        {
            case 256:
                imageSize = ImageSize.Size256x256;
                break;
            case 512:
                imageSize = ImageSize.Size512x512;
                break;
            default:
                imageSize = ImageSize.Size1024x1024;
                break;
        }

        var url = await _imageService.GetImageAsync(model.Prompt, imageSize);

        return Ok(url);
    }
}
