using image_upload_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace image_upload_api.Controllers
{
    public class ImageUpload
    {
        public IFormFile file { get; set; }
        public Guid sessionId { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ImageController(IImageService service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid sessionId)
        {
            return Ok(await service.GetImages(sessionId));
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] ImageUpload imageUpload)
        {
            var result = await service.UploadImage(imageUpload.file, imageUpload.sessionId);
            return Ok(result);
        }
    }
}
