using CloudOpsDashboard.Api.Hubs;
using CloudOpsDashboard.Core.Entities;
using CloudOpsDashboard.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CloudOpsDashboard.Api.Services
{
    public class AutoAlertGeneratorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<DashboardHub> _hubContext;
        private readonly ILogger<AutoAlertGeneratorService> _logger;

        public AutoAlertGeneratorService(
            IServiceScopeFactory scopeFactory,
            IHubContext<DashboardHub> hubContext,
            ILogger<AutoAlertGeneratorService> logger)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await GenerateAlertCycleAsync(stoppingToken);
                    await timer.WaitForNextTickAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Auto alert generator failed during execution.");
                }
            }
        }

        private async Task GenerateAlertCycleAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var instances = await context.CloudInstances
                .OrderBy(i => i.Id)
                .ToListAsync(cancellationToken);

            if (!instances.Any())
                return;

            var random = Random.Shared;
            var selectedInstance = instances[random.Next(instances.Count)];
            var eventType = random.Next(3); // 0 = CPU, 1 = Memory, 2 = Offline

            string title;
            string description;
            string severity;

            switch (eventType)
            {
                case 0:
                    selectedInstance.CpuUsage = Math.Round(random.NextDouble() * 20 + 76, 1);
                    selectedInstance.LastUpdated = DateTime.UtcNow;

                    title = "High CPU usage detected";
                    description = $"CPU usage exceeded threshold on {selectedInstance.Name}.";
                    severity = "High";
                    break;

                case 1:
                    selectedInstance.MemoryUsage = Math.Round(random.NextDouble() * 18 + 78, 1);
                    selectedInstance.LastUpdated = DateTime.UtcNow;

                    title = "Memory usage spike";
                    description = $"Memory usage increased significantly on {selectedInstance.Name}.";
                    severity = "Medium";
                    break;

                default:
                    if (!string.Equals(selectedInstance.Status, "Stopped", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedInstance.Status = "Stopped";
                    }

                    selectedInstance.CpuUsage = 0;
                    selectedInstance.MemoryUsage = 0;
                    selectedInstance.LastUpdated = DateTime.UtcNow;

                    title = "Instance offline";
                    description = $"{selectedInstance.Name} is currently stopped and unavailable.";
                    severity = "Critical";
                    break;
            }

            var duplicateExists = await context.CloudAlerts.AnyAsync(a =>
                !a.IsResolved &&
                a.Title == title &&
                a.Instance == selectedInstance.Name,
                cancellationToken);

            if (duplicateExists)
                return;

            var regionName = selectedInstance.RegionId switch
            {
                1 => "US East",
                2 => "Europe West",
                3 => "South Africa North",
                4 => "Asia Pacific South",
                _ => "Unknown Region"
            };

            context.CloudAlerts.Add(new CloudAlert
            {
                Title = title,
                Description = description,
                Severity = severity,
                Instance = selectedInstance.Name,
                Region = regionName,
                CreatedAt = DateTime.UtcNow,
                IsResolved = false
            });

            context.AuditLogs.Add(new AuditLog
            {
                Action = $"System generated alert: {title} on {selectedInstance.Name}",
                PerformedBy = "System",
                Timestamp = DateTime.UtcNow
            });

            await context.SaveChangesAsync(cancellationToken);
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate", cancellationToken);

            _logger.LogInformation("Auto alert generated: {Title} for {Instance}", title, selectedInstance.Name);
        }
    }
}
