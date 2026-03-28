using System.Text;
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
    public class AlertsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<DashboardHub> _hubContext;

        public AlertsController(AppDbContext context, IHubContext<DashboardHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = await _context.CloudAlerts
                .Where(a => !a.IsResolved)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return Ok(alerts);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAlerts()
        {
            var alerts = await _context.CloudAlerts
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return Ok(alerts);
        }

        [HttpGet("auditlogs")]
        public async Task<IActionResult> GetAuditLogs()
        {
            var logs = await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return Ok(logs);
        }

        [HttpGet("all/export")]
        public async Task<IActionResult> ExportAllAlerts()
        {
            var alerts = await _context.CloudAlerts
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Title,Severity,Status,Instance,Region,CreatedAt");

            foreach (var alert in alerts)
            {
                var title = EscapeCsv(alert.Title);
                var severity = EscapeCsv(alert.Severity);
                var status = alert.IsResolved ? "Resolved" : "Active";
                var instance = EscapeCsv(alert.Instance);
                var region = EscapeCsv(alert.Region);
                var createdAt = alert.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");

                sb.AppendLine($"{title},{severity},{status},{instance},{region},{createdAt}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", $"alert-history-{DateTime.UtcNow:yyyyMMdd-HHmmss}.csv");
        }

        [HttpPost("resolve/{id}")]
        public async Task<IActionResult> ResolveAlert(int id)
        {
            var alert = await _context.CloudAlerts.FindAsync(id);

            if (alert == null)
                return NotFound(new { message = "Alert not found." });

            if (alert.IsResolved)
                return BadRequest(new { message = "Alert is already resolved." });

            alert.IsResolved = true;

            _context.AuditLogs.Add(new AuditLog
            {
                Action = $"Resolved alert: {alert.Title ?? "Unknown Alert"}",
                PerformedBy = "Admin",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

            return Ok(new { message = "Alert resolved successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlert([FromBody] CloudAlert alert)
        {
            if (alert == null)
                return BadRequest(new { message = "Invalid alert data." });

            alert.CreatedAt = DateTime.UtcNow;
            alert.IsResolved = false;

            _context.CloudAlerts.Add(alert);

            _context.AuditLogs.Add(new AuditLog
            {
                Action = $"Created alert: {alert.Title ?? "Unknown Alert"}",
                PerformedBy = "System",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

            return Ok(new { message = "Alert created successfully." });
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "\"\"";

            var escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
    }
}