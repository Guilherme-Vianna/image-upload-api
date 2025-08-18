using image_upload_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace image_upload_api.Controllers
{
    public class ImageUpload
    {
        public IFormFile file { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ImageController(IImageService service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var image = await service.GetImages();
            return Ok(image);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] ImageUpload imageUpload)
        {
            var result = await service.UploadImage(imageUpload.file);
            return Ok(result);
        }
    }
}
