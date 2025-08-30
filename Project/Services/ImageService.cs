using CloudinaryDotNet;
using image_upload_api.Domain;
using image_upload_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;

namespace image_upload_api.Services
{
    public class ImageService : IImageService
    {
        private Cloudinary CloudService { get; set; }
        private ImageDatabaseContext Context { get; set; }
        private readonly IMinioClientFactory _minioClientFactory;

        public ImageService(IConfiguration configuration, ImageDatabaseContext context, IMinioClientFactory minioClientFactory)
        {
            this._minioClientFactory = minioClientFactory;
            Context = context;
            var url = configuration.GetSection("ExternalApi").GetValue<string>("ImageCloud:Url");
            var cloudinary = new Cloudinary(url);
            cloudinary.Api.Secure = true;
            CloudService = cloudinary;
        }

        public async Task<ImageData?> UpdateImageOnMinio(IFormFile image, Guid sessionId)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            using var client = _minioClientFactory.CreateClient();
            var originalObjectName = $"{sessionId}/{image.FileName}";

            var putArgsOriginal = new PutObjectArgs()
                .WithBucket("images")
                .WithObject(originalObjectName)
                .WithStreamData(image.OpenReadStream())
                .WithObjectSize(image.Length)
                .WithContentType(image.ContentType);

            var response = await client.PutObjectAsync(putArgsOriginal);

            var entity = new ImageData(response.ObjectName, sessionId);
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<Stream> LoadImageStreamFromMinio(string image)
        {
            var fileStream = new FileStream(
                Path.GetTempFileName(),
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None,
                4096,
                FileOptions.DeleteOnClose);

            var client = _minioClientFactory.CreateClient();

            var getArgs = new GetObjectArgs()
                .WithBucket("images")
                .WithObject(image)
                .WithCallbackStream(stream => stream.CopyTo(fileStream));

            await client.GetObjectAsync(getArgs);

            fileStream.Position = 0;

            return fileStream;
        }

        public async Task<List<ImageData>> GetImages(Guid sessionId)
        {
            return await Context.Images.Where(x => x.SessionId == sessionId).ToListAsync();
        }
    }
}
