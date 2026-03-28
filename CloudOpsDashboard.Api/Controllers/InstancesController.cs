using CloudOpsDashboard.Api.Hubs;
using CloudOpsDashboard.Core.Entities;
using CloudOpsDashboard.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CloudOpsDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstancesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<DashboardHub> _hubContext;

        public InstancesController(AppDbContext context, IHubContext<DashboardHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetInstances()
        {
            var instances = await _context.CloudInstances
                .OrderBy(i => i.Id)
                .ToListAsync();

            return Ok(instances);
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartInstance(int id)
        {
            var instance = await _context.CloudInstances.FindAsync(id);

            if (instance == null)
                return NotFound(new { message = "Instance not found." });

            if (string.Equals(instance.Status, "Running", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { message = "Instance is already running." });

            instance.Status = "Running";
            instance.LastUpdated = DateTime.UtcNow;

            _context.AuditLogs.Add(new AuditLog
            {
                Action = $"Started instance: {instance.Name}",
                PerformedBy = "Admin",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

            return Ok(new { message = "Instance started successfully." });
        }

        [HttpPost("{id}/stop")]
        public async Task<IActionResult> StopInstance(int id)
        {
            var instance = await _context.CloudInstances.FindAsync(id);

            if (instance == null)
                return NotFound(new { message = "Instance not found." });

            if (string.Equals(instance.Status, "Stopped", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { message = "Instance is already stopped." });

            instance.Status = "Stopped";
            instance.CpuUsage = 0;
            instance.MemoryUsage = 0;
            instance.LastUpdated = DateTime.UtcNow;

            _context.AuditLogs.Add(new AuditLog
            {
                Action = $"Stopped instance: {instance.Name}",
                PerformedBy = "Admin",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

            return Ok(new { message = "Instance stopped successfully." });
        }

        [HttpPost("{id}/restart")]
        public async Task<IActionResult> RestartInstance(int id)
        {
            var instance = await _context.CloudInstances.FindAsync(id);

            if (instance == null)
                return NotFound(new { message = "Instance not found." });

            instance.Status = "Running";
            instance.LastUpdated = DateTime.UtcNow;

            var random = new Random();
            instance.CpuUsage = Math.Round(random.NextDouble() * 40 + 20, 1);
            instance.MemoryUsage = Math.Round(random.NextDouble() * 40 + 30, 1);

            _context.AuditLogs.Add(new AuditLog
            {
                Action = $"Restarted instance: {instance.Name}",
                PerformedBy = "Admin",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

            return Ok(new { message = "Instance restarted successfully." });
        }
    }
}