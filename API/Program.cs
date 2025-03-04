using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
builder.Services.AddAutoMapper(typeof(MappingProfiles));
// Đăng ký DbContext với SQLite
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddControllers();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
