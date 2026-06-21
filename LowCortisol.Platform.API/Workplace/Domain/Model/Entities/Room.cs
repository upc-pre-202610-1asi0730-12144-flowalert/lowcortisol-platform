using LowCortisol.Platform.API.Shared.Domain.Model;
using LowCortisol.Platform.API.Workplace.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Workplace.Domain.Model.Entities;

public class Room : IEntity, IAuditableEntity
{
    private readonly List<DeviceGroup> _deviceGroups = [];

    private Room()
    {
        Name = string.Empty;
    }

    public Room(Guid id, Guid siteId, string name, RoomType type, OperationalStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Room name is required.", nameof(name));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        SiteId = siteId;
        Name = name.Trim();
        Type = type;
        Status = status;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public Guid SiteId { get; private set; }
    public string Name { get; private set; }
    public RoomType Type { get; private set; }
    public OperationalStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<DeviceGroup> DeviceGroups => _deviceGroups.AsReadOnly();

    public DeviceGroup AddDeviceGroup(Guid id, string name, ResourceType resourceType, OperationalStatus status)
    {
        if (_deviceGroups.Any(group => string.Equals(group.Name, name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Device group name already exists in this room.");

        var deviceGroup = new DeviceGroup(id, Id, name, resourceType, status);
        _deviceGroups.Add(deviceGroup);
        UpdatedAt = DateTime.UtcNow;

        return deviceGroup;
    }

    public void AttachDeviceGroup(DeviceGroup deviceGroup)
    {
        if (_deviceGroups.All(group => group.Id != deviceGroup.Id)) _deviceGroups.Add(deviceGroup);
    }

    public void ChangeStatus(OperationalStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
