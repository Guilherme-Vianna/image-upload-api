using image_upload_api.Commands;
using image_upload_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace image_upload_api.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class ImageController(IImageService service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid sessionId)
        {
            return Ok(await service.GetImages(sessionId));
        }

        [HttpGet("load")]
        public async Task<IActionResult> GetImage([FromQuery] string imageName)
        {
            var response = await service.LoadImageStreamFromMinio(imageName);
            return File(response, $"image/{imageName.Split(".")[1]}", imageName);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] ImageUploadCommand imageUpload)
        {
            var result = await service.UpdateImageOnMinio(imageUpload.file, imageUpload.sessionId);
            return Ok(result);
        }
    }
}
