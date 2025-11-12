using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;
using OJT_RAG.Services;
using OJT_RAG.Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Đăng ký PostgreSQL DbContext
builder.Services.AddDbContext<OJTRAGContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Các service khác
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddScoped<IJobPositionRepository, JobPositionRepository>();
builder.Services.AddScoped<IJobPositionService, JobPositionService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<ISemesterService, SemesterService>();

builder.Services.AddScoped<IMajorRepository, MajorRepository>();
builder.Services.AddScoped<IMajorService, MajorService>();

builder.Services.AddScoped<IJobDescriptionRepository, JobDescriptionRepository>();
builder.Services.AddScoped<IJobDescriptionService, JobDescriptionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
