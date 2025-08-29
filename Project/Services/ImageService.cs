using CloudinaryDotNet;
using image_upload_api.Domain;
using image_upload_api.Responses;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using Image = image_upload_api.Domain.Entities.Image;

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

        public async Task<Image?> UploadImageOnCloudService(IFormFile image, Guid sessionId)
        {
            throw new NotImplementedException();
            //await using var stream = image.OpenReadStream();

            //var uploadParams = new ImageUploadParams
            //{
            //    File = new FileDescription(image.FileName, stream)
            //};

            //var response = await CloudService.UploadAsync(uploadParams);
            //if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;

            //var imageUrl = response.SecureUrl.AbsoluteUri;
            //var newImage = new Image(imageUrl, sessionId);
            //await Context.AddAsync(newImage);
            //await Context.SaveChangesAsync();

            //return newImage;
        }

        public async Task<Image?> UpdateImageOnMinio(IFormFile image, Guid sessionId)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            using var client = _minioClientFactory.CreateClient();

            var putArgs = new PutObjectArgs()
                .WithBucket("images")
                .WithObject(image.FileName)
                .WithStreamData(image.OpenReadStream())
                .WithObjectSize(image.Length)
                .WithContentType(image.ContentType);

            var response = await client.PutObjectAsync(putArgs);

            var entity = new Image(response.ObjectName, sessionId, response.ObjectName);
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<ImageResponse> LoadImageFromMinio(string image)
        {
            using var client = _minioClientFactory.CreateClient();
            using var imageStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket("images")
                .WithObject(image)
                .WithCallbackStream((stream) =>
                {
                    stream.CopyTo(imageStream);
                });

            await client.GetObjectAsync(getObjectArgs);
            var file = imageStream.ToArray();
            var extension = image.Split(".")[1];
            var response = new ImageResponse(extension, file);
            return response;
        }

        public async Task<Stream> LoadImageStreamFromMinio(string image)
        {
            var tempFilePath = Path.GetTempFileName();
            var fileStream = new FileStream(
                tempFilePath,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None,
                4096,
                FileOptions.DeleteOnClose);

            var client = _minioClientFactory.CreateClient();

            var statArgs = new StatObjectArgs()
                .WithBucket("images")
                .WithObject(image);
            await client.StatObjectAsync(statArgs);

            var getArgs = new GetObjectArgs()
                .WithBucket("images")
                .WithObject(image)
                .WithCallbackStream(stream => stream.CopyTo(fileStream));

            await client.GetObjectAsync(getArgs);

            fileStream.Position = 0;

            return fileStream;
        }

        public async Task<List<Image>> GetImages(Guid sessionId)
        {
            return await Context.Images.Where(x => x.SessionId == sessionId).ToListAsync();
        }
    }
}
