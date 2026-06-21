namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.ValueObjects;

public sealed record PhysicalTarget(
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid DeviceId,
    Guid? ValveId)
{
    public bool HasDevice => DeviceId != Guid.Empty;
    public bool HasValve => ValveId.HasValue && ValveId.Value != Guid.Empty;
}
