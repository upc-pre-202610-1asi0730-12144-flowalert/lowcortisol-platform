namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record RequestIncidentMitigationCommand(
    Guid IncidentId,
    Guid DeviceId,
    Guid ValveId,
    string RequestedBy,
    string Reason);
