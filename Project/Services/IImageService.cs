using image_upload_api.Domain.Entities;
using image_upload_api.Responses;

namespace image_upload_api.Services
{
    public interface IImageService
    {
        public Task<List<Image>> GetImages(Guid sessionId);
        public Task<Image?> UploadImageOnCloudService(IFormFile image, Guid sessionId);
        public Task<Image?> UpdateImageOnMinio(IFormFile image, Guid sessionId);
        public Task<ImageResponse> LoadImageFromMinio(string image);
        public Task<Stream> LoadImageStreamFromMinio(string image);
    }
}
