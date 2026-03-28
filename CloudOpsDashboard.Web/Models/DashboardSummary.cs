namespace CloudOpsDashboard.Web.Models;

public class DashboardSummary
{
    public int TotalInstances { get; set; }
    public int RunningInstances { get; set; }
    public int StoppedInstances { get; set; }
    public int TotalAlerts { get; set; }
    public int CriticalAlerts { get; set; }
    public double AverageCpuUsage { get; set; }
    public double AverageMemoryUsage { get; set; }
}
