namespace image_upload_api.Services
{
    public interface IImageService
    {
        public Task<List<string>> GetImages();
        public Task<string> UploadImage(IFormFile image);
    }
}
