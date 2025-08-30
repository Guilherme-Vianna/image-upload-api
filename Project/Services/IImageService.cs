using image_upload_api.Domain.Entities;

namespace image_upload_api.Services
{
    public interface IImageService
    {
        public Task<List<ImageData>> GetImages(Guid sessionId);
        public Task<ImageData?> UpdateImageOnMinio(IFormFile image, Guid sessionId);
        public Task<Stream> LoadImageStreamFromMinio(string image);
    }
}
