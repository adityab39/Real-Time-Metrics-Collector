using Microsoft.EntityFrameworkCore;
using RealTimeMetricsApi.Data;
using RealTimeMetricsApi.Services;
using RealTimeMetricsApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MetricsDb"));

builder.Services.AddScoped<MetricService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.AppThresholds.Add(new AppThreshold
    {
        AppId = "iot-sensor",
        CpuThreshold = 80,
        MemoryThreshold = 500
    });
    db.SaveChanges();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers(); 

app.Run();