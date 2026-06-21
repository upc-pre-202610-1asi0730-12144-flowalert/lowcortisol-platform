using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;

public class Threshold : IEntity, IAuditableEntity
{
    private Threshold()
    {
        Name = string.Empty;
        Unit = string.Empty;
    }

    public Threshold(
        Guid id,
        Guid? siteId,
        Guid? roomId,
        Guid? deviceGroupId,
        Guid? sensorId,
        ResourceType resourceType,
        string name,
        ThresholdOperator @operator,
        decimal limitValue,
        string unit,
        AnomalySeverity severity,
        bool isActive = true)
    {
        if (siteId is null && roomId is null && deviceGroupId is null && sensorId is null)
            throw new ArgumentException("Threshold must be associated to at least one scope.");
        if (!Enum.IsDefined(resourceType))
            throw new ArgumentException("Resource type is required.", nameof(resourceType));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Threshold name is required.", nameof(name));
        if (!Enum.IsDefined(@operator))
            throw new ArgumentException("Threshold operator is required.", nameof(@operator));
        if (limitValue < 0)
            throw new ArgumentException("Threshold limit value cannot be negative.", nameof(limitValue));
        if (string.IsNullOrWhiteSpace(unit)) throw new ArgumentException("Unit is required.", nameof(unit));
        if (!Enum.IsDefined(severity))
            throw new ArgumentException("Threshold severity is required.", nameof(severity));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        SiteId = siteId;
        RoomId = roomId;
        DeviceGroupId = deviceGroupId;
        SensorId = sensorId;
        ResourceType = resourceType;
        Name = name.Trim();
        Operator = @operator;
        LimitValue = limitValue;
        Unit = unit.Trim();
        Severity = severity;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid? SiteId { get; private set; }
    public Guid? RoomId { get; private set; }
    public Guid? DeviceGroupId { get; private set; }
    public Guid? SensorId { get; private set; }
    public ResourceType ResourceType { get; private set; }
    public string Name { get; private set; }
    public ThresholdOperator Operator { get; private set; }
    public decimal LimitValue { get; private set; }
    public string Unit { get; private set; }
    public AnomalySeverity Severity { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public bool AppliesTo(ConsumptionReading reading)
    {
        if (!IsActive || ResourceType != reading.ResourceType) return false;
        if (SiteId.HasValue && SiteId.Value != reading.SiteId) return false;
        if (RoomId.HasValue && RoomId.Value != reading.RoomId) return false;
        if (DeviceGroupId.HasValue && DeviceGroupId.Value != reading.DeviceGroupId) return false;
        if (SensorId.HasValue && SensorId.Value != reading.SensorId) return false;

        return true;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
