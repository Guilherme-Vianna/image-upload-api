using image_upload_api.Domain;
using image_upload_api.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<ImageDatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration["DB_CONNECTION"]));
builder.Services.AddHostedService<ImageUploaderService>();

var minioHost = builder.Configuration["MINIO_HOST"];
var minioPort = builder.Configuration["MINIO_PORT"];
var minioSSL = builder.Configuration["MINIO_SSL"];
var minioUser = builder.Configuration["MINIO_USERNAME"];
var minioPass = builder.Configuration["MINIO_PASSWORD"];

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioHost, int.Parse(minioPort))
    .WithCredentials(minioUser, minioPass)
    .WithSSL(bool.Parse(minioSSL))
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
