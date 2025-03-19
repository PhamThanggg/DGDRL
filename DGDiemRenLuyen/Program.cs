using DGDiemRenLuyen.Data;
using Microsoft.EntityFrameworkCore;
using DGDiemRenLuyen.Extentions;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DTOs.ModelValidation;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Connect DB
builder.Services.AddDbContext<SQLDRLContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:Cnn"]);
});

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

var keycloakSettings = builder.Configuration.GetSection("Keycloak");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://account.hnue.edu.vn/realms/hnue_sso";
        options.Audience = "account"; 
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://account.hnue.edu.vn/realms/hnue_sso",
            ValidAudience = "account",
            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
            {
                // Lấy public key từ Keycloak
                var client = new HttpClient();
                var keySetUrl = "https://account.hnue.edu.vn/realms/hnue_sso/protocol/openid-connect/certs";
                var response = client.GetStringAsync(keySetUrl).Result;
                var keySet = new JsonWebKeySet(response);
                return keySet.Keys;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

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
