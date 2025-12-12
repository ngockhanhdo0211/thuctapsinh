using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using UnifiedLearningApi.Data;
using UnifiedLearningApi.Mappings;
using UnifiedLearningApi.Middlewares;
using UnifiedLearningApi.Repositories;
using UnifiedLearningApi.Repositories.Interfaces;
using UnifiedLearningApi.Services;
using UnifiedLearningApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

/* ========== 1. SERILOG ========== */
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/api.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

/* ========== 2. KESTREL PORTS ========== */
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5079);
    options.ListenLocalhost(7166, listen => listen.UseHttps());
});

/* ========== 3. DATABASE ========== */
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

/* ========== 4. AUTOMAPPER ========== */
builder.Services.AddAutoMapper(typeof(MappingProfile));

/* ========== 5. DEPENDENCY INJECTION ========== */
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentTreeService, CommentTreeService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IAuthService, AuthService>();

/* ========== 6. JWT AUTHENTICATION ========== */
var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

/* ========== 7. API VERSIONING ========== */
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Explorer để Swagger nhận version
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // v1, v2, …
    options.SubstituteApiVersionInUrl = true;
});

/* ========== 8. SWAGGER ========== */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "UnifiedLearningApi",
        Version = "v1"
    });

    // Thêm nút AUTH cho Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập token theo format: Bearer {your token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

/* ========== 9. CONTROLLERS ========== */
builder.Services.AddControllers();

var app = builder.Build();

/* ========== 10. MIGRATION AUTO APPLY ========== */
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

/* ========== 11. SWAGGER UI ========== */
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UnifiedLearningApi v1");
});

/* ========== 12. MIDDLEWARES ========== */
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
