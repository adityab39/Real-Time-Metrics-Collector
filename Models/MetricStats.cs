namespace RealTimeMetricsApi.Models
{
    public class MetricStats
    {
        public string AppId   { get; set; } = string.Empty;
        public int    Count   { get; set; }

        public double AvgCpu  { get; set; }
        public double MinCpu  { get; set; }
        public double MaxCpu  { get; set; }

        public double AvgMemory { get; set; }
        public double MinMemory { get; set; }
        public double MaxMemory { get; set; }

        public double AvgLatency { get; set; }
        public double MinLatency { get; set; }
        public double MaxLatency { get; set; }

        public int CriticalCount { get; set; }
    }
}