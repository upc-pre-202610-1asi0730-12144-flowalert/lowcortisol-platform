namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record CloseValveForIncidentResource(
    Guid DeviceId,
    Guid ValveId,
    string RequestedBy,
    string? Reason);
