namespace image_upload_api.Domain.Entities
{
    public class Image : Audit<Guid>
    {
        public Image()
        {

        }
        public string Url { get; set; }
    }
}
