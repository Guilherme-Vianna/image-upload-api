using image_upload_api.Domain.Entities;

namespace image_upload_api.Services
{
    public interface IImageService
    {
        public Task<List<Image>> GetImages(Guid sessionId);
        public Task<Image?> UploadImage(IFormFile image, Guid sessionId);
    }
}
