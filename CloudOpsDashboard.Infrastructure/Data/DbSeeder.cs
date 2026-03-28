using CloudOpsDashboard.Core.Entities;

namespace CloudOpsDashboard.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Regions.Any() || context.CloudInstances.Any() || context.CloudAlerts.Any())
            return;

        var regions = new List<Region>
        {
            new Region { Name = "US East", Code = "us-east-1" },
            new Region { Name = "Europe West", Code = "eu-west-1" },
            new Region { Name = "South Africa North", Code = "af-south-1" },
            new Region { Name = "Asia Pacific Southeast", Code = "ap-southeast-1" }
        };
        if (!context.AppUsers.Any())
        {
            context.AppUsers.AddRange(
                new AppUser
                {
                    Username = "admin",
                    Password = "admin123",
                    Role = "Admin"
                },
                new AppUser
                {
                    Username = "viewer",
                    Password = "viewer123",
                    Role = "Viewer"
                }
            );
        }
        context.Regions.AddRange(regions);
        context.SaveChanges();

        var instances = new List<CloudInstance>
        {
            new CloudInstance
            {
                Name = "Compute-USE1-01",
                Status = "Running",
                InstanceType = "t3.large",
                RegionId = regions[0].Id,
                CpuUsage = 42.5,
                MemoryUsage = 68.3,
                UptimePercentage = 99.98,
                LastUpdated = DateTime.UtcNow
            },
            new CloudInstance
            {
                Name = "Compute-EUW1-02",
                Status = "Running",
                InstanceType = "t3.xlarge",
                RegionId = regions[1].Id,
                CpuUsage = 76.4,
                MemoryUsage = 81.1,
                UptimePercentage = 99.92,
                LastUpdated = DateTime.UtcNow
            },
            new CloudInstance
            {
                Name = "Compute-AFS1-03",
                Status = "Stopped",
                InstanceType = "t3.medium",
                RegionId = regions[2].Id,
                CpuUsage = 0.0,
                MemoryUsage = 0.0,
                UptimePercentage = 97.45,
                LastUpdated = DateTime.UtcNow
            },
            new CloudInstance
            {
                Name = "Compute-APS1-04",
                Status = "Running",
                InstanceType = "m5.large",
                RegionId = regions[3].Id,
                CpuUsage = 63.9,
                MemoryUsage = 59.4,
                UptimePercentage = 99.87,
                LastUpdated = DateTime.UtcNow
            }
        };

        context.CloudInstances.AddRange(instances);
        context.SaveChanges();

        var alerts = new List<CloudAlert>
        {
            new CloudAlert
{
    Title = "Memory usage spike",
    Description = "Memory usage increased significantly on Compute-USE1-01.",
    Severity = "Medium",
    Instance = "Compute-USE1-01",
    Region = "US East",
    CreatedAt = DateTime.UtcNow,
    IsResolved = true
},
new CloudAlert
{
    Title = "Instance offline",
    Description = "Compute-AFS1-03 is currently stopped and unavailable.",
    Severity = "Critical",
    Instance = "Compute-AFS1-03",
    Region = "South Africa North",
    CreatedAt = DateTime.UtcNow,
    IsResolved = false
},
new CloudAlert
{
    Title = "High CPU usage detected",
    Description = "CPU usage exceeded 75% threshold on Compute-EUW1-02.",
    Severity = "High",
    Instance = "Compute-EUW1-02",
    Region = "Europe West",
    CreatedAt = DateTime.UtcNow,
    IsResolved = true
}
        };

        context.CloudAlerts.AddRange(alerts);
        context.SaveChanges();
    }
}
