using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;

public class ConsumptionReading : IEntity, IAuditableEntity
{
    private ConsumptionReading()
    {
        Unit = string.Empty;
    }

    public ConsumptionReading(
        Guid id,
        Guid siteId,
        Guid roomId,
        Guid deviceGroupId,
        Guid deviceId,
        Guid sensorId,
        ResourceType resourceType,
        decimal value,
        string unit,
        DateTime? capturedAt)
    {
        if (siteId == Guid.Empty) throw new ArgumentException("Site id is required.", nameof(siteId));
        if (roomId == Guid.Empty) throw new ArgumentException("Room id is required.", nameof(roomId));
        if (deviceGroupId == Guid.Empty)
            throw new ArgumentException("Device group id is required.", nameof(deviceGroupId));
        if (deviceId == Guid.Empty) throw new ArgumentException("Device id is required.", nameof(deviceId));
        if (sensorId == Guid.Empty) throw new ArgumentException("Sensor id is required.", nameof(sensorId));
        if (!Enum.IsDefined(resourceType))
            throw new ArgumentException("Resource type is required.", nameof(resourceType));
        if (value < 0) throw new ArgumentException("Reading value cannot be negative.", nameof(value));
        if (string.IsNullOrWhiteSpace(unit)) throw new ArgumentException("Unit is required.", nameof(unit));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        SiteId = siteId;
        RoomId = roomId;
        DeviceGroupId = deviceGroupId;
        DeviceId = deviceId;
        SensorId = sensorId;
        ResourceType = resourceType;
        Value = value;
        Unit = unit.Trim();
        CapturedAt = capturedAt ?? DateTime.UtcNow;
        Status = ReadingStatus.Normal;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid SiteId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid DeviceGroupId { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid SensorId { get; private set; }
    public ResourceType ResourceType { get; private set; }
    public decimal Value { get; private set; }
    public string Unit { get; private set; }
    public DateTime CapturedAt { get; private set; }
    public ReadingStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void MarkStatus(ReadingStatus status)
    {
        if (!Enum.IsDefined(status)) return;

        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
