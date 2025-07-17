using Microsoft.EntityFrameworkCore;
using RealTimeMetricsApi.Models;

namespace RealTimeMetricsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Metric> Metrics { get; set; }
        public DbSet<AppThreshold> AppThresholds { get; set; }
    }
}