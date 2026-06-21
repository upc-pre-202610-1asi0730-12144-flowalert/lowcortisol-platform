using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;

public class Valve : IEntity, IAuditableEntity
{
    private Valve()
    {
        Name = string.Empty;
    }

    public Valve(Guid id, Guid deviceId, string name, DeviceResourceType resourceType, ValveStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Valve name is required.", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        DeviceId = deviceId;
        Name = name.Trim();
        ResourceType = resourceType;
        Status = status;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }
    public string Name { get; private set; }
    public DeviceResourceType ResourceType { get; private set; }
    public ValveStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public bool CanBeClosed() => Status == ValveStatus.Open;

    public bool CanBeOpened() => Status == ValveStatus.Closed;

    public void Close()
    {
        if (!CanBeClosed()) return;

        Status = ValveStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Open()
    {
        if (!CanBeOpened()) return;

        Status = ValveStatus.Open;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsMaintenance()
    {
        Status = ValveStatus.Maintenance;
        UpdatedAt = DateTime.UtcNow;
    }
}
