namespace IASquad.Poc.AzureOpenAi.Models.Image;

public class ImageIndexViewModel
{
    public string Prompt { get; set; } = "";

    public int Size { get; set; } = 1024;

    public string ImageUrl { get; set; } = "";
}
