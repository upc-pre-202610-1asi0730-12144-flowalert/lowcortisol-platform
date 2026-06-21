using LowCortisol.Platform.API.Shared.Domain.Model;
using LowCortisol.Platform.API.Workplace.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Workplace.Domain.Model.Entities;

public class DeviceGroup : IEntity, IAuditableEntity
{
    private DeviceGroup()
    {
        Name = string.Empty;
    }

    public DeviceGroup(Guid id, Guid roomId, string name, ResourceType resourceType, OperationalStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device group name is required.", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        RoomId = roomId;
        Name = name.Trim();
        ResourceType = resourceType;
        Status = status;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string Name { get; private set; }
    public ResourceType ResourceType { get; private set; }
    public OperationalStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void ChangeStatus(OperationalStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
