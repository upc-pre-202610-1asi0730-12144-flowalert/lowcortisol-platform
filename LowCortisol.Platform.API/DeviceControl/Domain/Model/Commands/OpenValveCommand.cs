namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;

public record OpenValveCommand(
    Guid DeviceId,
    Guid ValveId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    string RequestedBy,
    string Reason);
