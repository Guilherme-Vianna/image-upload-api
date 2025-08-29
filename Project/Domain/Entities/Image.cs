namespace image_upload_api.Domain.Entities
{
    public class Image : Audit
    {
        public Image()
        {

        }

        public Image(string url, Guid sessionId, string previewUrl)
        {
            Url = url;
            SessionId = sessionId;
            PreviewUrl = previewUrl;
        }

        public string? PreviewUrl { get; set; }
        public string Url { get; set; }
        public string? CloudnaryLink { get; set; }
        public Guid SessionId { get; set; }
    }
}
