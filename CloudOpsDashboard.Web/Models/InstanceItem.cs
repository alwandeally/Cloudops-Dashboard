namespace CloudOpsDashboard.Web.Models
{
    public class InstanceItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string InstanceType { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public double UptimePercentage { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
