using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;

public class Sensor : IEntity, IAuditableEntity
{
    private Sensor()
    {
        Name = string.Empty;
    }

    public Sensor(
        Guid id,
        Guid deviceId,
        string name,
        DeviceResourceType resourceType,
        DeviceStatus status,
        decimal? lastReadingValue)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Sensor name is required.", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        DeviceId = deviceId;
        Name = name.Trim();
        ResourceType = resourceType;
        Status = status;
        LastReadingValue = lastReadingValue;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }
    public string Name { get; private set; }
    public DeviceResourceType ResourceType { get; private set; }
    public DeviceStatus Status { get; private set; }
    public decimal? LastReadingValue { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
