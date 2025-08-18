namespace image_upload_api.Domain.Entities
{
    public class Audit<T>
    {
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
