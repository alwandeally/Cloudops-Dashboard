using CloudOpsDashboard.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudOpsDashboard.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Region> Regions => Set<Region>();
    public DbSet<CloudInstance> CloudInstances => Set<CloudInstance>();
    public DbSet<InstanceMetric> InstanceMetrics => Set<InstanceMetric>();
    public DbSet<CloudAlert> CloudAlerts => Set<CloudAlert>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CloudInstance>()
            .HasOne(ci => ci.Region)
            .WithMany()
            .HasForeignKey(ci => ci.RegionId);

        modelBuilder.Entity<InstanceMetric>()
            .HasOne(im => im.CloudInstance)
            .WithMany()
            .HasForeignKey(im => im.CloudInstanceId);

        modelBuilder.Entity<CloudAlert>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(a => a.Severity)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(a => a.Instance)
                .HasMaxLength(100);

            entity.Property(a => a.Region)
                .HasMaxLength(100);

            entity.Property(a => a.CreatedAt)
                .IsRequired();

            entity.Property(a => a.IsResolved)
                .IsRequired();

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(20);
            });


        });
    }
}