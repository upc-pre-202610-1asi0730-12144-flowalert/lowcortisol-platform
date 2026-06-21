using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Model.ValueObjects;

using MonitoringResourceType = LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects.ResourceType;
using WorkplaceResourceType = LowCortisol.Platform.API.Workplace.Domain.Model.ValueObjects.ResourceType;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

public sealed class AppDbContext
{
    public AppDbContext()
    {
        Seed();
    }

    public List<Site> Sites { get; } = [];
    public List<Room> Rooms { get; } = [];
    public List<DeviceGroup> DeviceGroups { get; } = [];
    public List<Device> Devices { get; } = [];
    public List<Sensor> Sensors { get; } = [];
    public List<Valve> Valves { get; } = [];
    public List<DeviceCommand> DeviceCommands { get; } = [];
    public List<ValveOperation> ValveOperations { get; } = [];
    public List<CommandExecution> CommandExecutions { get; } = [];
    public List<CommandAuditEntry> CommandAuditEntries { get; } = [];
    public List<ConsumptionReading> ConsumptionReadings { get; } = [];
    public List<Threshold> Thresholds { get; } = [];
    public List<Anomaly> Anomalies { get; } = [];
    public List<Alert> Alerts { get; } = [];
    public List<Incident> Incidents { get; } = [];
    public List<NotificationChannel> NotificationChannels { get; } = [];
    public List<AlertDelivery> AlertDeliveries { get; } = [];

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    private void Seed()
    {
        if (Sites.Count > 0) return;

        var miraflores = new Site(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Residencial Miraflores",
            "Av. La Paz 1240, Lima",
            SiteType.Residential,
            OperationalStatus.Active);

        var cocina = miraflores.AddRoom(
            Guid.Parse("22222222-2222-2222-2222-222222222221"),
            "Zona Cocina",
            RoomType.Kitchen,
            OperationalStatus.Active);
        var torre = miraflores.AddRoom(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Torre A",
            RoomType.Custom,
            OperationalStatus.Active);

        var gasGroup = cocina.AddDeviceGroup(
            Guid.Parse("33333333-3333-3333-3333-333333333331"),
            "Grupo Gas Principal",
            WorkplaceResourceType.Gas,
            OperationalStatus.Active);
        var waterGroup = torre.AddDeviceGroup(
            Guid.Parse("33333333-3333-3333-3333-333333333332"),
            "Grupo Agua Torre A",
            WorkplaceResourceType.Water,
            OperationalStatus.Active);

        Sites.Add(miraflores);
        Rooms.AddRange(miraflores.Rooms);
        DeviceGroups.AddRange(cocina.DeviceGroups);
        DeviceGroups.AddRange(torre.DeviceGroups);

        var hub = new Device(
            Guid.Parse("44444444-4444-4444-4444-444444444441"),
            "Dispositivo Hub Norte",
            "LC-HUB-001",
            DeviceType.Hub,
            DeviceStatus.Online,
            gasGroup.Id);
        var gasSensor = new Sensor(
            Guid.Parse("55555555-5555-5555-5555-555555555551"),
            hub.Id,
            "Sensor de gas MQ-02",
            DeviceResourceType.Gas,
            DeviceStatus.Online,
            136);
        var gasValve = new Valve(
            Guid.Parse("66666666-6666-6666-6666-666666666661"),
            hub.Id,
            "Valvula de corte principal",
            DeviceResourceType.Gas,
            ValveStatus.Open);
        var waterHub = new Device(
            Guid.Parse("44444444-4444-4444-4444-444444444442"),
            "Hub Hidraulico Torre A",
            "LC-HUB-002",
            DeviceType.Hub,
            DeviceStatus.Online,
            waterGroup.Id);
        var waterSensor = new Sensor(
            Guid.Parse("55555555-5555-5555-5555-555555555552"),
            waterHub.Id,
            "Sensor agua torre A",
            DeviceResourceType.Water,
            DeviceStatus.Online,
            284);

        Devices.AddRange([hub, waterHub]);
        Sensors.AddRange([gasSensor, waterSensor]);
        Valves.Add(gasValve);

        var gasThreshold = new Threshold(
            Guid.Parse("77777777-7777-7777-7777-777777777771"),
            miraflores.Id,
            cocina.Id,
            gasGroup.Id,
            gasSensor.Id,
            MonitoringResourceType.Gas,
            "Gas critical kitchen threshold",
            ThresholdOperator.GreaterOrEqual,
            120,
            "m3",
            AnomalySeverity.Critical);

        var waterThreshold = new Threshold(
            Guid.Parse("77777777-7777-7777-7777-777777777772"),
            miraflores.Id,
            torre.Id,
            waterGroup.Id,
            waterSensor.Id,
            MonitoringResourceType.Water,
            "Water warning tower threshold",
            ThresholdOperator.GreaterThan,
            320,
            "L",
            AnomalySeverity.Warning);

        var gasReading = new ConsumptionReading(
            Guid.Parse("88888888-8888-8888-8888-888888888881"),
            miraflores.Id,
            cocina.Id,
            gasGroup.Id,
            hub.Id,
            gasSensor.Id,
            MonitoringResourceType.Gas,
            136,
            "m3",
            DateTime.UtcNow.AddMinutes(-20));

        var waterReading = new ConsumptionReading(
            Guid.Parse("88888888-8888-8888-8888-888888888882"),
            miraflores.Id,
            torre.Id,
            waterGroup.Id,
            waterHub.Id,
            waterSensor.Id,
            MonitoringResourceType.Water,
            284,
            "L",
            DateTime.UtcNow.AddMinutes(-12));

        var gasAnomaly = Anomaly.FromReading(gasReading, gasThreshold);
        gasReading.MarkStatus(ReadingStatus.Critical);

        var inAppChannel = new NotificationChannel(
            Guid.Parse("99999999-9999-9999-9999-999999999991"),
            "In-app operations channel",
            NotificationChannelType.InApp);
        var criticalAlert = Alert.FromCriticalAnomaly(
            gasAnomaly.Id,
            gasAnomaly.ReadingId,
            gasAnomaly.SiteId,
            gasAnomaly.RoomId,
            gasAnomaly.DeviceGroupId,
            gasAnomaly.DeviceId,
            gasAnomaly.SensorId,
            gasAnomaly.ResourceType.ToString(),
            gasAnomaly.Value,
            gasAnomaly.LimitValue,
            gasAnomaly.Unit,
            gasAnomaly.DetectedAt);
        var criticalDelivery = criticalAlert.RegisterDelivery(inAppChannel, Recipient.OperationsDesk());
        var criticalIncident = Incident.FromAlert(criticalAlert);

        Thresholds.AddRange([gasThreshold, waterThreshold]);
        ConsumptionReadings.AddRange([gasReading, waterReading]);
        Anomalies.Add(gasAnomaly);
        NotificationChannels.Add(inAppChannel);
        Alerts.Add(criticalAlert);
        Incidents.Add(criticalIncident);
        AlertDeliveries.Add(criticalDelivery);
    }
}
