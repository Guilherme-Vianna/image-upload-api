namespace image_upload_api.Domain.Entities
{
    public class Image : Audit
    {
        public Image()
        {

        }

        public Image(string url, Guid sessionId)
        {
            Url = url;
            SessionId = sessionId;
        }

        public string Url { get; set; }
        public Guid SessionId { get; set; }
    }
}
