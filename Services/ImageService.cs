using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace image_upload_api.Services
{
    public class ImageService : IImageService
    {
        public Cloudinary cloudService { get; set; }
        public ImageService(IConfiguration configuration)
        {
            var url = configuration.GetSection("ExternalApi").GetValue<string>("ImageCloud:Url");
            Cloudinary cloudinary = new Cloudinary(url);
            cloudinary.Api.Secure = true;
            cloudService = cloudinary;
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            using (var stream = image.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(image.FileName, stream)
                };

                var response = await cloudService.UploadAsync(uploadParams);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.AssetId.ToString();
                }
            }
            return "Error";
        }

        public async Task<List<string>> GetImages()
        {
            var getResourceParams = new GetResourceParams("cld-sample-5")
            {
                QualityAnalysis = true
            };
            var getResourceResult = cloudService.GetResource(getResourceParams);
            var resultJson = getResourceResult.JsonObj;
            var response = new List<string>() { "" };
            return response;
        }
    }
}
