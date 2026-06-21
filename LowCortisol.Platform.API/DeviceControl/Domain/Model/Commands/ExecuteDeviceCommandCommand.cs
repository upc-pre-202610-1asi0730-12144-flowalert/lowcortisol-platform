namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;

public record ExecuteDeviceCommandCommand(
    Guid DeviceId,
    Guid? ValveId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid? IncidentId,
    string CommandType,
    string Source,
    string Reason,
    string RequestedBy);
