using System;
using System.Collections.Generic;
using System.Text;

namespace CloudOpsDashboard.Core.Entities;

public class CloudInstance
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Running";
    public string InstanceType { get; set; } = string.Empty;

    public int RegionId { get; set; }
    public Region? Region { get; set; }

    public double CpuUsage { get; set; }
    public double MemoryUsage { get; set; }
    public double UptimePercentage { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}