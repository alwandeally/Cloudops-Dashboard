namespace CloudOpsDashboard.Web.Models
{
    public class HealthStatusItem
    {
        public string ApiStatus { get; set; } = string.Empty;
        public string DatabaseStatus { get; set; } = string.Empty;
        public string SignalRStatus { get; set; } = string.Empty;
        public DateTime LastRefreshUtc { get; set; }
    }
}
