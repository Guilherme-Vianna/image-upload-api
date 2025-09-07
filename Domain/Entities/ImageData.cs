namespace image_upload_api.Domain.Entities
{
    public class ImageData : Audit
    {
        public ImageData()
        {

        }

        public ImageData(string url, Guid sessionId)
        {
            Url = url;
            SessionId = sessionId;
        }

        public string Url { get; set; }
        public string? CloudnaryLink { get; set; }
        public Guid SessionId { get; set; }
    }
}
