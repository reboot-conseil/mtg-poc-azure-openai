using Azure.AI.OpenAI;
using IASquad.Poc.AzureOpenAi.Models.Image;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IASquad.Poc.AzureOpenAi.Controllers;

public class ImageController : Controller
{

    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] ImageIndexViewModel model)
    {
        return View(model);
    }

    [HttpPost("Generate")]
    public async Task<IActionResult> Generate([FromForm] ImageGenerateViewModel model)
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
        var vm = new ImageIndexViewModel
        {
            Prompt = model.Prompt,
            ImageUrl = url,
            Size = model.Size,
        };

        return RedirectToAction("Index", vm);
    }
}
