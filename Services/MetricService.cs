using RealTimeMetricsApi.Data;
using RealTimeMetricsApi.Models;

namespace RealTimeMetricsApi.Services
{
    public class MetricService
    {
        private readonly AppDbContext _context;

        public MetricService(AppDbContext context)
        {
            _context = context;
        }

        public Metric CheckAndSaveMetric(Metric metric)
        {
            var threshold = _context.AppThresholds.FirstOrDefault(t => t.AppId == metric.AppId);

            if (threshold != null)
            {
                if (metric.Cpu > threshold.CpuThreshold || metric.Memory > threshold.MemoryThreshold)
                {
                    metric.IsCritical = true;
                }
            }

            _context.Metrics.Add(metric);
            _context.SaveChanges();
            return metric;
        }

        public List<Metric> GetMetrics(string appId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Metrics.AsQueryable();

            if (!string.IsNullOrWhiteSpace(appId))
                query = query.Where(m => m.AppId == appId);

            if (startDate.HasValue)
                query = query.Where(m => m.Timestamp >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(m => m.Timestamp <= endDate.Value);

            return query
                .OrderByDescending(m => m.Timestamp)
                .ToList();
        }
    }
}