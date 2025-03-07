using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddAutoMapper(typeof(MappingProfiles));
// Đăng ký DbContext với SQLite
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerDocumentation();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Gọi phương thức khởi tạo DB khi ứng dụng khởi động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<StoreContext>();
        context.Database.Migrate(); // Áp dụng migration tự động
        StoreContextSeed.SeedAsynce(context, loggerFactory);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration Error: {ex.Message}");
    }
}

app.UseSwaggerDocumentation();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
