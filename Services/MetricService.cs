using RealTimeMetricsApi.Data;
using RealTimeMetricsApi.Models;
using CsvHelper;
using System.Globalization;
using System.IO;

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

        public MetricStats? GetStats(string appId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Metrics.AsQueryable();

            // mandatory filter
            query = query.Where(m => m.AppId == appId);

            // optional date filters
            if (startDate.HasValue) query = query.Where(m => m.Timestamp >= startDate.Value);
            if (endDate.HasValue)   query = query.Where(m => m.Timestamp <= endDate.Value);

            if (!query.Any()) return null;  

            return new MetricStats
            {
                AppId         = appId,
                Count         = query.Count(),

                AvgCpu        = query.Average(m => m.Cpu),
                MinCpu        = query.Min(m => m.Cpu),
                MaxCpu        = query.Max(m => m.Cpu),

                AvgMemory     = query.Average(m => m.Memory),
                MinMemory     = query.Min(m => m.Memory),
                MaxMemory     = query.Max(m => m.Memory),

                AvgLatency    = query.Average(m => m.Latency),
                MinLatency    = query.Min(m => m.Latency),
                MaxLatency    = query.Max(m => m.Latency),

                CriticalCount = query.Count(m => m.IsCritical)
            };
        }

        public byte[] ExportMetricsToCsv(string appId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Metrics.AsQueryable();

            query = query.Where(m => m.AppId == appId);

            if (startDate.HasValue)
                query = query.Where(m => m.Timestamp >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(m => m.Timestamp <= endDate.Value);

            var metrics = query.OrderBy(m => m.Timestamp).ToList();

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(metrics);
            writer.Flush();

            return memoryStream.ToArray();
        }
    }
}