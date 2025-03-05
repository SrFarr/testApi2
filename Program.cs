using Microsoft.EntityFrameworkCore;
using UserAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Konfigurasi DbContext
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(); // Wajib ada untuk API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Untuk Swagger UI (opsional)

var app = builder.Build();

// **Pastikan Swagger bisa diakses di Production**
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Pastikan ini ada agar controller bisa diakses

app.Run();
