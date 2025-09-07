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
        private const string BucketName = "images";

        public ImageService(IConfiguration configuration, ImageDatabaseContext context, IMinioClientFactory minioClientFactory)
        {
            _minioClientFactory = minioClientFactory;
            Context = context;
        }

        private async Task EnsureBucketExistsAsync(IMinioClient client)
        {
            bool found = await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(BucketName));
            if (!found)
            {
                await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName));
            }
        }

        public async Task<ImageData?> UpdateImageOnMinio(IFormFile image, Guid sessionId)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            using var client = _minioClientFactory.CreateClient();
            await EnsureBucketExistsAsync(client);

            var originalObjectName = $"{sessionId}/{image.FileName}";

            var putArgsOriginal = new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(originalObjectName)
                .WithStreamData(image.OpenReadStream())
                .WithObjectSize(image.Length)
                .WithContentType(image.ContentType);

            await client.PutObjectAsync(putArgsOriginal);

            var entity = new ImageData(originalObjectName, sessionId);
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<string> ShareImage(Guid sessionId, Guid imageId)
        {
            var image = await Context.Images.FirstAsync(x => x.Id == imageId);

            using var client = _minioClientFactory.CreateClient();
            await EnsureBucketExistsAsync(client);

            var args = new PresignedGetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(image.Url)
                .WithExpiry(3600);

            var url = await client.PresignedGetObjectAsync(args);
            return url;
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

            using var client = _minioClientFactory.CreateClient();
            await EnsureBucketExistsAsync(client);

            var getArgs = new GetObjectArgs()
                .WithBucket(BucketName)
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
