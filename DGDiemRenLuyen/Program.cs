using DGDiemRenLuyen.Data;
using Microsoft.EntityFrameworkCore;
using DGDiemRenLuyen.Extentions;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DTOs.ModelValidation;
using Microsoft.Extensions.FileProviders;
using DGDiemRenLuyen.DTOs.Responses;
using System.Text.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Connect DB
builder.Services.AddDbContext<SQLDRLContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:Cnn"]);
});

// http call api
builder.Services.AddHttpClient();

// vaidate
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});

// web roof lưu file
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

// HttpContext
builder.Services.AddHttpContextAccessor();

// scan service repository
builder.Services
    .AddRepositories(Assembly.GetExecutingAssembly())
    .AddServices(Assembly.GetExecutingAssembly());

// Add services to the container.
builder.Services.AddControllers();

// keyclock sso
var keycloakSettings = builder.Configuration.GetSection("Keycloak");


// authen jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),

        ClockSkew = TimeSpan.Zero // Không cho lệch thời gian
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse(); // Bỏ res mặc định
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status401Unauthorized.ToString(),
                Messages = "Bạn chưa đăng nhập hoặc token không hợp lệ.",
                Data = null
            });

            return context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// 403
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 403)
    {
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new ApiResponse<string>
        {
            StatusCode = StatusCodes.Status403Forbidden.ToString(),
            Messages = "Bạn không có quyền truy cập chức năng này.",
            Data = null
        });

        await context.Response.WriteAsync(result);
    }
});

app.UseAuthentication();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "uploads")),
    RequestPath = "/uploads"
});

app.Run();
