namespace image_upload_api.Responses
{
    public class ImageResponse
    {
        public ImageResponse(string extension, byte[] image)
        {
            Extension = extension;
            Image = image;
        }

        public string Extension { get; }
        public byte[] Image { get; }
    }
}
