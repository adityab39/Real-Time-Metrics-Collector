namespace RealTimeMetricsApi.Models
{
    public class Metric
    {
        public int Id { get; set; }
        public string AppId { get; set; }
        public double Cpu { get; set; }
        public double Memory { get; set; }
        public double Latency { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsCritical { get; set; } = false;
    }
}