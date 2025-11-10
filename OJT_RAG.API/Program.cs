using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;
using OJT_RAG.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Đăng ký PostgreSQL DbContext
builder.Services.AddDbContext<OJTRAGContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Các service khác
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMajorRepository, MajorRepository>();
builder.Services.AddScoped<IMajorService, MajorService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
