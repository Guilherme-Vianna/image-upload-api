namespace image_upload_api.Domain.Entities;

public class ImageProcessingJob : Audit
{
    public ImageProcessingJob()
    {

    }

    public ImageProcessingJob(string source, Guid sessionId)
    {
        Source = source;
        SessionId = sessionId;
    }

    public string Source { get; set; }
    public Guid SessionId { get; set; }
}