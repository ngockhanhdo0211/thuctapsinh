using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UnifiedLearningApi.Data;
using UnifiedLearningApi.Mappings;
using UnifiedLearningApi.Middlewares;
using UnifiedLearningApi.Repositories;
using UnifiedLearningApi.Repositories.Interfaces;
using UnifiedLearningApi.Services;
using UnifiedLearningApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/api.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// 2. Kestrel Ports
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5079);          // HTTP
    options.ListenLocalhost(7166, listen => listen.UseHttps()); // HTTPS
});

// 3. Database
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 4. AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 5. DI
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentTreeService, CommentTreeService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// 6. SWAGGER (CHUẨN KHÔNG VERSIONING)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "UnifiedLearningApi",
        // Swagger/OpenAPI requires a semantic-style version string like "v1"
        Version = "v1"
    });
});


builder.Services.AddControllers();

var app = builder.Build();

// 7. Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// 8. Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UnifiedLearningApi v1");
});

// 9. Middleware
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
