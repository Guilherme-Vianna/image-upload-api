using image_upload_api.Domain;
using image_upload_api.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Minio;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresDatabase");

builder.Services.AddDbContext<ImageDatabaseContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddHostedService<ImageUploaderService>();
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(builder.Configuration.GetValue<string>("MinioDatabase:Hostname") + ":" +
                  builder.Configuration.GetValue<string>("MinioDatabase:Port"))
    .WithCredentials(
        builder.Configuration.GetValue<string>("MinioDatabase:Username"),
        builder.Configuration.GetValue<string>("MinioDatabase:Password"))
    .WithSSL(false)
    .Build());

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 5_000_000_000;
    o.MemoryBufferThreshold = 1 * 1024 * 1024;
    o.BufferBody = false;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ImageDatabaseContext>();
        db.Database.Migrate();
        logger.LogInformation("Database migrated.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database migration failed.");
        throw;
    }
}

app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyOrigin();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
