namespace RealTimeMetricsApi.Models
{
    public class AppThreshold
    {
        public int Id { get; set; }
        public string AppId { get; set; }
        public double CpuThreshold { get; set; }
        public double MemoryThreshold { get; set; }
    }
}