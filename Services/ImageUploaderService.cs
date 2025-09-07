namespace image_upload_api.Services;

public class ImageUploaderService(IServiceProvider serviceProvider) : BackgroundService
{
    private Timer? timer = null;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        timer = new Timer(UploadImage, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        return Task.CompletedTask;
    }

    private void UploadImage(object? state)
    {
        //var imageService = serviceProvider.GetService<IImageService>();
        //imageService.UploadImage();
    }
}