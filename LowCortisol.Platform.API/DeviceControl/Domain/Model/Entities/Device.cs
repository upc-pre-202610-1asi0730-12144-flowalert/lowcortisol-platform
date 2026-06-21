using LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;

public class Device : IEntity, IAuditableEntity
{
    private Device()
    {
        Name = string.Empty;
        SerialNumber = string.Empty;
    }

    public Device(
        Guid id,
        string name,
        string serialNumber,
        DeviceType type,
        DeviceStatus status,
        Guid? deviceGroupId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(serialNumber))
            throw new ArgumentException("Device serial number is required.", nameof(serialNumber));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name.Trim();
        SerialNumber = serialNumber.Trim();
        Type = type;
        Status = status;
        DeviceGroupId = deviceGroupId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string SerialNumber { get; private set; }
    public DeviceType Type { get; private set; }
    public DeviceStatus Status { get; private set; }
    public Guid? DeviceGroupId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void AssignToDeviceGroup(Guid deviceGroupId)
    {
        DeviceGroupId = deviceGroupId;
        UpdatedAt = DateTime.UtcNow;
    }
}
