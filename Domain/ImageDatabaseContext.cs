using image_upload_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace image_upload_api.Domain
{
    public class ImageDatabaseContext : DbContext
    {
        public ImageDatabaseContext(DbContextOptions<ImageDatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<ImageData> Images { get; set; }
    }
}
