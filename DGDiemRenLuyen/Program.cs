using DGDiemRenLuyen.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connect DB
builder.Services.AddDbContext<SQLDRLContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:Cnn"]);
});

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
