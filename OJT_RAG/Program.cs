using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OJT_RAG.Data;
using OJT_RAG.Services;
using OJT_RAG.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Cloudinary service
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton<ICloudinaryService, CloudinaryService>();


// Add Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OJT_RAG API",
        Version = "v1"
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OJT_RAG API V1");
        c.RoutePrefix = "swagger"; // truy cập tại https://localhost:7171/swagger
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
