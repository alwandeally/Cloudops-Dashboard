using System;
using System.Collections.Generic;
using System.Text;


namespace CloudOpsDashboard.Core.Entities;

public class InstanceMetric
{
    public int Id { get; set; }

    public int CloudInstanceId { get; set; }
    public CloudInstance? CloudInstance { get; set; }

    public double CpuUsage { get; set; }
    public double MemoryUsage { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}