using image_upload_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace image_upload_api.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<Image> Images { get; set; }
    }
}
