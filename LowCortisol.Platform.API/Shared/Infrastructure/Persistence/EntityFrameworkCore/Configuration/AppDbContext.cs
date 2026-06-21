using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Site> Sites => Set<Site>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<DeviceGroup> DeviceGroups => Set<DeviceGroup>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<Valve> Valves => Set<Valve>();
    public DbSet<DeviceCommand> DeviceCommands => Set<DeviceCommand>();
    public DbSet<ValveOperation> ValveOperations => Set<ValveOperation>();
    public DbSet<CommandExecution> CommandExecutions => Set<CommandExecution>();
    public DbSet<CommandAuditEntry> CommandAuditEntries => Set<CommandAuditEntry>();
    public DbSet<ConsumptionReading> ConsumptionReadings => Set<ConsumptionReading>();
    public DbSet<Threshold> Thresholds => Set<Threshold>();
    public DbSet<Anomaly> Anomalies => Set<Anomaly>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<NotificationChannel> NotificationChannels => Set<NotificationChannel>();
    public DbSet<AlertDelivery> AlertDeliveries => Set<AlertDelivery>();
    public DbSet<IncidentAction> IncidentActions => Set<IncidentAction>();
    public DbSet<IncidentAssignment> IncidentAssignments => Set<IncidentAssignment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        builder.UseSnakeCaseNamingConvention();
    }
}
