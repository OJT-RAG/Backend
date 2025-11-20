using Microsoft.EntityFrameworkCore;
using OJT_RAG.Repositories;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;
using OJT_RAG.Services;
using OJT_RAG.Repositories.Repositories;
using OJT_RAG.Services.Implementations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using OJT_RAG.Services.UserService;

var builder = WebApplication.CreateBuilder(args);


// 👇 Thêm DateOnly Converter
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
    });

//  Đăng ký PostgreSQL DbContext
builder.Services.AddDbContext<OJTRAGContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Ép Swagger hiểu DateOnly là string "yyyy-MM-dd"
    c.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date"
    });

    c.MapType<DateOnly?>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Nullable = true
    });
});


// Services
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

builder.Services.AddScoped<IJobTitleOverviewRepository, JobTitleOverviewRepository>();
builder.Services.AddScoped<IJobTitleOverviewService, JobTitleOverviewService>();

builder.Services.AddScoped<IJobBookmarkRepository, JobBookmarkRepository>();
builder.Services.AddScoped<IJobBookmarkService, JobBookmarkService>();

builder.Services.AddScoped<IFinalreportRepository, FinalreportRepository>();
builder.Services.AddScoped<IFinalreportService, FinalreportService>();

builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
builder.Services.AddScoped<IChatRoomService, ChatRoomService>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddScoped<ISemesterCompanyRepository, SemesterCompanyRepository>();
builder.Services.AddScoped<ISemesterCompanyService, SemesterCompanyService>();

builder.Services.AddScoped<IDocumentTagRepository, DocumentTagRepository>();
builder.Services.AddScoped<IDocumentTagService, DocumentTagService>();

builder.Services.AddScoped<IOjtDocumentRepository, OjtDocumentRepository>();
builder.Services.AddScoped<IOjtDocumentService, OjtDocumentService>();


builder.Services.AddScoped<ICompanyDocumentRepository, CompanyDocumentRepository>();
builder.Services.AddScoped<ICompanyDocumentService, CompanyDocumentService>();
builder.Services.AddSingleton<GoogleDriveService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();


// 👇 DateOnly Converters
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return DateOnly.ParseExact(value!, Format);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrEmpty(value) ? null : DateOnly.ParseExact(value!, Format);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString(Format));
        else
            writer.WriteNullValue();
    }
}
