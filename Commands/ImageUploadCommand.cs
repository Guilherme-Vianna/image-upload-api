namespace image_upload_api.Commands
{
    public class ImageUploadCommand
    {
        public IFormFile file { get; set; }
        public Guid sessionId { get; set; }
    }
}
