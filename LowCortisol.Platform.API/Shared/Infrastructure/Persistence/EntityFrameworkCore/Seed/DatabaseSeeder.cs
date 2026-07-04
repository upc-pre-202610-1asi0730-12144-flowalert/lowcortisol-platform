using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using LowCortisol.Platform.API.Support.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

using MonitoringResourceType = LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects.ResourceType;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;
using WorkplaceResourceType = LowCortisol.Platform.API.Workplace.Domain.Model.ValueObjects.ResourceType;

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Seed;

public sealed class DatabaseSeeder
{
    private readonly AppDbContext _context;

    public DatabaseSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedPlanCatalogAsync(cancellationToken);
        await SeedSupportCatalogAsync(cancellationToken);

        if (await _context.Sites.AnyAsync(cancellationToken)) return;

        var site = new Site(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Residencial Miraflores",
            "Av. La Paz 1240, Lima",
            SiteType.Residential,
            OperationalStatus.Active);

        var kitchen = site.AddRoom(
            Guid.Parse("22222222-2222-2222-2222-222222222221"),
            "Zona Cocina",
            RoomType.Kitchen,
            OperationalStatus.Active);

        var tower = site.AddRoom(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Torre A",
            RoomType.Custom,
            OperationalStatus.Active);

        var gasGroup = kitchen.AddDeviceGroup(
            Guid.Parse("33333333-3333-3333-3333-333333333331"),
            "Grupo Gas Principal",
            WorkplaceResourceType.Gas,
            OperationalStatus.Active);

        var waterGroup = tower.AddDeviceGroup(
            Guid.Parse("33333333-3333-3333-3333-333333333332"),
            "Grupo Agua Torre A",
            WorkplaceResourceType.Water,
            OperationalStatus.Active);

        var hub = new Device(
            Guid.Parse("44444444-4444-4444-4444-444444444441"),
            "Hub Miraflores",
            "LC-HUB-001",
            DeviceType.Hub,
            DeviceStatus.Online,
            gasGroup.Id);

        var gasSensor = new Sensor(
            Guid.Parse("55555555-5555-5555-5555-555555555551"),
            hub.Id,
            "Sensor gas cocina central",
            DeviceResourceType.Gas,
            DeviceStatus.Online,
            136);

        var waterSensor = new Sensor(
            Guid.Parse("55555555-5555-5555-5555-555555555552"),
            hub.Id,
            "Sensor agua torre A",
            DeviceResourceType.Water,
            DeviceStatus.Online,
            284);

        var valve = new Valve(
            Guid.Parse("66666666-6666-6666-6666-666666666661"),
            hub.Id,
            "Valvula gas cocina central",
            DeviceResourceType.Gas,
            ValveStatus.Open);

        var threshold = new Threshold(
            Guid.Parse("77777777-7777-7777-7777-777777777771"),
            site.Id,
            kitchen.Id,
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
            site.Id,
            tower.Id,
            waterGroup.Id,
            waterSensor.Id,
            MonitoringResourceType.Water,
            "Water warning tower threshold",
            ThresholdOperator.GreaterOrEqual,
            260,
            "L",
            AnomalySeverity.Warning);

        var gasReading = new ConsumptionReading(
            Guid.Parse("88888888-8888-8888-8888-888888888881"),
            site.Id,
            kitchen.Id,
            gasGroup.Id,
            hub.Id,
            gasSensor.Id,
            MonitoringResourceType.Gas,
            136,
            "m3",
            DateTime.UtcNow.AddMinutes(-20));

        var waterReading = new ConsumptionReading(
            Guid.Parse("88888888-8888-8888-8888-888888888882"),
            site.Id,
            tower.Id,
            waterGroup.Id,
            hub.Id,
            waterSensor.Id,
            MonitoringResourceType.Water,
            284,
            "L",
            DateTime.UtcNow.AddMinutes(-15));

        var criticalAnomaly = Anomaly.FromReading(gasReading, threshold);
        gasReading.MarkStatus(ReadingStatus.Critical);

        var warningAnomaly = Anomaly.FromReading(waterReading, waterThreshold);
        waterReading.MarkStatus(ReadingStatus.Warning);

        var inAppChannel = NotificationChannel.DefaultInApp(
            Guid.Parse("99999999-9999-9999-9999-999999999991"));

        var alert = Alert.FromCriticalAnomaly(
            criticalAnomaly.Id,
            gasReading.Id,
            criticalAnomaly.SiteId,
            criticalAnomaly.RoomId,
            criticalAnomaly.DeviceGroupId,
            criticalAnomaly.DeviceId,
            criticalAnomaly.SensorId,
            criticalAnomaly.ResourceType.ToString(),
            criticalAnomaly.Value,
            criticalAnomaly.LimitValue,
            criticalAnomaly.Unit,
            criticalAnomaly.DetectedAt);

        alert.RegisterDelivery(inAppChannel, Recipient.OperationsDesk());

        var incident = Incident.FromAlert(alert);
        incident.Assign("operations", "Operations desk");
        incident.RegisterAction(
            "inspection",
            "Initial in-app response registered for critical anomaly.",
            "Operations desk");

        await _context.Sites.AddAsync(site, cancellationToken);
        await _context.Devices.AddAsync(hub, cancellationToken);
        await _context.Sensors.AddRangeAsync([gasSensor, waterSensor], cancellationToken);
        await _context.Valves.AddAsync(valve, cancellationToken);
        await _context.Thresholds.AddRangeAsync([threshold, waterThreshold], cancellationToken);
        await _context.ConsumptionReadings.AddRangeAsync([gasReading, waterReading], cancellationToken);
        await _context.Anomalies.AddRangeAsync([criticalAnomaly, warningAnomaly], cancellationToken);
        await _context.NotificationChannels.AddAsync(inAppChannel, cancellationToken);
        await _context.Alerts.AddAsync(alert, cancellationToken);
        await _context.Incidents.AddAsync(incident, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedPlanCatalogAsync(CancellationToken cancellationToken)
    {
        if (await _context.Plans.AnyAsync(cancellationToken)) return;

        var essential = new PlanAggregate(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
            "essential",
            "Essential",
            "Para monitoreo inicial de una sede.",
            49,
            "PEN",
            "monthly",
            1,
            5);
        var professional = new PlanAggregate(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
            "professional",
            "Professional",
            "Para equipos que operan varias sedes.",
            129,
            "PEN",
            "monthly",
            5,
            35,
            true);
        var enterprise = new PlanAggregate(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
            "enterprise",
            "Enterprise",
            "Para operaciones con alta criticidad.",
            299,
            "PEN",
            "monthly",
            25,
            250);

        essential.AddFeature(Guid.Parse("abababab-abab-abab-abab-ababababab01"), "Monitoreo de agua y gas", "Control de consumo por sede.");
        essential.AddFeature(Guid.Parse("abababab-abab-abab-abab-ababababab02"), "Alertas operativas", "Alertas dentro de la aplicacion.");
        professional.AddFeature(Guid.Parse("abababab-abab-abab-abab-ababababab03"), "Sedes y dispositivos ampliados", "Operacion de varias sedes y dispositivos.");
        professional.AddFeature(Guid.Parse("abababab-abab-abab-abab-ababababab04"), "Reportes de consumo", "Reportes por periodo y recurso.");
        professional.AddFeature(Guid.Parse("abababab-abab-abab-abab-ababababab05"), "Canales de alerta configurables", "Configuracion operativa de notificaciones.");
        enterprise.AddFeature(Guid.Parse("abababab-abab-abab-abab-ababababab06"), "Operacion multi sede", "Cobertura para operaciones distribuidas.");
        enterprise.AddFeature(Guid.Parse("abababab-abab-abab-abab-ababababab07"), "Alertas prioritarias", "Priorizacion de incidentes criticos.");
        enterprise.AddFeature(Guid.Parse("abababab-abab-abab-abab-ababababab08"), "Soporte operativo avanzado", "Atencion para continuidad operativa.");

        await _context.Plans.AddRangeAsync([essential, professional, enterprise], cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedSupportCatalogAsync(CancellationToken cancellationToken)
    {
        if (!await _context.SupportAgents.AnyAsync(cancellationToken))
        {
            await _context.SupportAgents.AddRangeAsync(
                [
                    new SupportAgent(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), "Lucia Ramos", "Monitoreo", SupportAgentStatus.Available),
                    new SupportAgent(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), "Mateo Silva", "Dispositivos", SupportAgentStatus.Busy)
                ],
                cancellationToken);
        }

        if (!await _context.KnowledgeArticles.AnyAsync(cancellationToken))
        {
            await _context.KnowledgeArticles.AddRangeAsync(
                [
                    new KnowledgeArticle(
                        Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                        "Como interpretar una alerta critica",
                        "Guia para priorizar eventos de agua y gas con impacto operativo.",
                        "Alertas",
                        24,
                        "Revisa el recurso afectado, la sede, la valvula asociada y el limite superado antes de iniciar la mitigacion."),
                    new KnowledgeArticle(
                        Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                        "Buenas practicas para sensores",
                        "Recomendaciones para mantener lecturas estables en sedes activas.",
                        "Dispositivos",
                        18,
                        "Cada valvula operativa debe tener un sensor del mismo recurso y pertenecer al mismo grupo fisico.")
                ],
                cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
