using Microsoft.AspNetCore.Mvc;
using web_blog.Services;

namespace web_blog.Controllers;

[Route("api/Image")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly ImageService _imageService;

    public ImageController(ImageService imageService)
    {
        _imageService = imageService;
    }
    
    [HttpGet("avatar/{filename}")]
    public async Task<IActionResult> GetImage(string filename)
    {
        var img = await _imageService.GetImageAsync(filename);
        if (img == null)
        {
            return NotFound();
        }

        return img;
    }
}