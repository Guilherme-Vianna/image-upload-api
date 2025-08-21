using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using image_upload_api.Domain;
using image_upload_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace image_upload_api.Services
{
    public class ImageService : IImageService
    {
        public Cloudinary cloudService { get; set; }
        public ImageDatabaseContext Context { get; set; }
        public ImageService(IConfiguration configuration, ImageDatabaseContext context)
        {
            Context = context;
            var url = configuration.GetSection("ExternalApi").GetValue<string>("ImageCloud:Url");
            Cloudinary cloudinary = new Cloudinary(url);
            cloudinary.Api.Secure = true;
            cloudService = cloudinary;
        }

        public async Task<Image?> UploadImage(IFormFile image, Guid sessionId)
        {
            await using var stream = image.OpenReadStream();
            
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream)
            };

            var response = await cloudService.UploadAsync(uploadParams);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;
            
            var imageUrl = response.SecureUrl.AbsoluteUri;
            var newImage = new Image(imageUrl, sessionId);
            await Context.AddAsync(newImage);
            await Context.SaveChangesAsync();

            return newImage;
        }

        public async Task<List<Image>> GetImages(Guid sessionId)
        {
            return await Context.Images.Where(x => x.SessionId == sessionId).ToListAsync();
        }
    }
}
