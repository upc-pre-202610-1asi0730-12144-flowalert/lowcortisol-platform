using LowCortisol.Platform.API.Monitoring.Domain.Model.Events;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;

public class Anomaly : IEntity, IAuditableEntity
{
    private Anomaly()
    {
        Unit = string.Empty;
        Description = string.Empty;
    }

    private Anomaly(
        Guid id,
        ConsumptionReading reading,
        Threshold threshold,
        string description)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        ReadingId = reading.Id;
        ThresholdId = threshold.Id;
        SiteId = reading.SiteId;
        RoomId = reading.RoomId;
        DeviceGroupId = reading.DeviceGroupId;
        DeviceId = reading.DeviceId;
        SensorId = reading.SensorId;
        ResourceType = reading.ResourceType;
        Value = reading.Value;
        LimitValue = threshold.LimitValue;
        Unit = reading.Unit;
        Severity = threshold.Severity;
        Status = AnomalyStatus.Open;
        Description = description;
        DetectedAt = DateTime.UtcNow;
        CreatedAt = DetectedAt;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid ReadingId { get; private set; }
    public Guid ThresholdId { get; private set; }
    public Guid SiteId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid DeviceGroupId { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid SensorId { get; private set; }
    public ResourceType ResourceType { get; private set; }
    public decimal Value { get; private set; }
    public decimal LimitValue { get; private set; }
    public string Unit { get; private set; }
    public AnomalySeverity Severity { get; private set; }
    public AnomalyStatus Status { get; private set; }
    public string Description { get; private set; }
    public DateTime DetectedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static Anomaly FromReading(ConsumptionReading reading, Threshold threshold)
    {
        var description =
            $"{threshold.Name}: {reading.Value} {reading.Unit} reached threshold {threshold.LimitValue} {threshold.Unit}.";

        return new Anomaly(Guid.NewGuid(), reading, threshold, description);
    }

    public void Resolve()
    {
        Status = AnomalyStatus.Resolved;
        UpdatedAt = DateTime.UtcNow;
    }

    public CriticalAnomalyDetected ToCriticalEvent() =>
        new(
            Id,
            ReadingId,
            SiteId,
            RoomId,
            DeviceGroupId,
            DeviceId,
            SensorId,
            ResourceType,
            Value,
            LimitValue,
            Unit,
            DetectedAt);
}
