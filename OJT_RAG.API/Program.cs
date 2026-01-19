using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using OJT_RAG.Repositories;
using OJT_RAG.Repositories.Context;
using OJT_RAG.Repositories.Entities;
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

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);

var builder = WebApplication.CreateBuilder(args);

// ====================== LẤY BIẾN MÔI TRƯỜNG & CẤU HÌNH DB ======================
// 1. Lấy chuỗi mặc định từ appsettings.json (Dùng cho Local)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

// 2. Lấy biến môi trường từ Railway (Dùng cho Production)
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var railwayConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

// Ưu tiên biến môi trường ConnectionStrings__DefaultConnection nếu có (Do ta cấu hình thủ công)
if (!string.IsNullOrEmpty(railwayConnectionString))
{
    connectionString = railwayConnectionString;
}
// Nếu không, thử check biến DATABASE_URL (Mặc định của Railway)
else if (!string.IsNullOrEmpty(databaseUrl))
{
    // Kiểm tra xem nó là dạng URI (postgres://...) hay dạng Key=Value (Host=...)
    if (databaseUrl.StartsWith("postgres://"))
    {
        try 
        {
            var uri = new Uri(databaseUrl);
            var userInfo = uri.UserInfo.Split(':');
            connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};" +
                               $"Username={userInfo[0]};Password={userInfo[1]};" +
                               $"SSL Mode=Prefer;Trust Server Certificate=true;";
        }
        catch 
        {
            // Nếu parse lỗi, cứ gán thẳng vào, có thể nó là chuỗi connection string nhưng đặt tên biến nhầm
            connectionString = databaseUrl;
        }
    }
    else
    {
        // Nếu không bắt đầu bằng postgres://, nghĩa là nó đã là dạng Host=... rồi
        connectionString = databaseUrl;
    }
}

Console.WriteLine($"--> Using Connection String: {connectionString}"); // Log ra để debug (sẽ hiện trong log Railway)

// ====================== CẤU HÌNH JWT KEY ======================
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
    ?? "Chuoi_Mac_Dinh_Neu_Khong_Thay_Key_Phai_Dai_Hon_16_Ky_Tu_Nhe"; 

// ====================== DATABASE (Npgsql 8.x + ENUM) ======================
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<UserRole>("user_role_enum");
dataSourceBuilder.EnableUnmappedTypes();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<OJTRAGContext>(options =>
{
    options.UseNpgsql(dataSource);
});

// ====================== CẤU HÌNH JWT AUTHENTICATION ======================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                message = "Bạn chưa đăng nhập hoặc phiên làm việc đã hết hạn. Vui lòng đăng nhập lại!"
            });
            return context.Response.WriteAsync(result);
        }
    };
});

// ====================== CORS ======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://frontend-ojt-544c.vercel.app") // Thay domain frontend của bạn vào đây
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ====================== CONTROLLERS + JSON OPTIONS ======================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());

        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; 
        options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment(); 
    });

// ====================== SWAGGER CONFIG ======================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
    c.MapType<DateOnly?>(() => new OpenApiSchema { Type = "string", Format = "date", Nullable = true });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Dán Token vào đây (Không cần gõ chữ Bearer)"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] {}
        }
    });
});

// ====================== DEPENDENCY INJECTION ======================
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
builder.Services.AddScoped<ICompanyDocumentTagRepository, CompanyDocumentTagRepository>();
builder.Services.AddScoped<ICompanyDocumentTagService, CompanyDocumentTagService>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IOjtDocumentTagRepository, OjtDocumentTagRepository>();
builder.Services.AddScoped<IOjtDocumentTagService, OjtDocumentTagService>();
builder.Services.AddScoped<UserChatService>();
builder.Services.AddScoped<IUserChatRepository, UserChatRepository>();
builder.Services.AddScoped<GoogleAuthService>();
builder.Services.AddSingleton<GoogleDriveService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

// ====================== APP BUILD & MIDDLEWARE ======================
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseRouting();

// Thứ tự quan trọng: Authentication trước Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Vì Railway quản lý Port qua biến môi trường PORT, ta để mặc định app.Run()
// Nó sẽ tự động lắng nghe theo cấu hình trong Dockerfile hoặc biến môi trường
app.Run();

// ====================== CONVERTERS ======================
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
        var value = reader.GetString();
        return string.IsNullOrEmpty(value) ? null : DateOnly.ParseExact(value!, Format);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue) writer.WriteStringValue(value.Value.ToString(Format));
        else writer.WriteNullValue();
    }
}
