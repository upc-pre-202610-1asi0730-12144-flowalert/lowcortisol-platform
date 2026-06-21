namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;

public record CloseValveForIncidentCommand(
    Guid IncidentId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid DeviceId,
    Guid ValveId,
    string RequestedBy,
    string Reason);
