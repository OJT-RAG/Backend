using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OJT_RAG.API.Hubs;
using OJT_RAG.Repositories;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Repositories.Repositories;
using OJT_RAG.Services;
using OJT_RAG.Services.Auth;
using OJT_RAG.Services.Implementations;
using OJT_RAG.Services.Interfaces;
using OJT_RAG.Services.UserService;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


// ---------------------- JWT CONFIG ----------------------
var jwtKey =
    Environment.GetEnvironmentVariable("JWT_KEY")
    ?? builder.Configuration["Jwt:Key"];

var jwtIssuer =
    Environment.GetEnvironmentVariable("JWT_ISSUER")
    ?? builder.Configuration["Jwt:Issuer"];

var jwtAudience =
    Environment.GetEnvironmentVariable("JWT_AUDIENCE")
    ?? builder.Configuration["Jwt:Audience"];

Console.WriteLine($"JWT_KEY loaded: {!string.IsNullOrWhiteSpace(jwtKey)}");
Console.WriteLine("JWT SOURCE = " +
    (Environment.GetEnvironmentVariable("JWT_KEY") != null ? "ENV" : "APPSETTINGS"));

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("JWT_KEY is missing");

if (string.IsNullOrWhiteSpace(jwtIssuer))
    throw new Exception("JWT_ISSUER is missing");

if (string.IsNullOrWhiteSpace(jwtAudience))
    throw new Exception("JWT_AUDIENCE is missing");

// 🔑 Inject lại vào IConfiguration cho toàn app
builder.Configuration["Jwt:Key"] = jwtKey;
builder.Configuration["Jwt:Issuer"] = jwtIssuer;
builder.Configuration["Jwt:Audience"] = jwtAudience;
// ---------------------- CORS ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "https://localhost:7031"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// ---------------------- JSON OPTIONS ----------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
    });

// ---------------------- DATABASE ----------------------
builder.Services.AddDbContext<OJTRAGContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// ---------------------- SWAGGER ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
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

    // Enable JWT input on Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Nhập JWT Token vào đây",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// ---------------------- AUTHENTICATION (JWT) ----------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
    // 📌 Custom bắt lỗi 401, 403
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse(); // Ngăn response mặc định

            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = 401,
                message = "Token không hợp lệ hoặc thiếu token."
            });

            return context.Response.WriteAsync(result);
        },

        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = 403,
                message = "Bạn không có quyền truy cập tài nguyên này."
            });

            return context.Response.WriteAsync(result);
        }
    };
});

// ---------------------- AUTHORIZATION ----------------------
builder.Services.AddAuthorization();

// ---------------------- DEPENDENCY INJECTION ----------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddScoped<IJobPositionRepository, JobPositionRepository>();
builder.Services.AddScoped<IJobPositionService, JobPositionService>();

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

builder.Services.AddScoped<ICompanyDocumentTagRepository, CompanyDocumentTagRepository>();
builder.Services.AddScoped<ICompanyDocumentTagService, CompanyDocumentTagService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<IUserChatRepository, UserChatRepository>();
builder.Services.AddScoped<UserChatService>();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

builder.Services.AddSignalR();


builder.Services.AddSingleton<GoogleDriveService>();

// ---------------------- BUILD APP ----------------------
var app = builder.Build();

// ---------------------- MIDDLEWARE ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.MapHub<UserChatHub>("/hubs/user-chat");


// MUST BE BEFORE Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


// ---------------------- JSON CONVERTERS ----------------------
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateOnly.ParseExact(reader.GetString()!, Format);

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
}

public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    private const string Format = "yyyy-MM-dd";
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        return string.IsNullOrEmpty(str) ? null : DateOnly.ParseExact(str!, Format);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue) writer.WriteStringValue(value.Value.ToString(Format));
        else writer.WriteNullValue();
    }
}
