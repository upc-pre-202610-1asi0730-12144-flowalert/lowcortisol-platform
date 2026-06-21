namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

public record ValveOperationResource(
    Guid Id,
    Guid ValveId,
    Guid DeviceId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid? IncidentId,
    string ResourceType,
    string PreviousStatus,
    string TargetStatus,
    string Reason,
    string Source,
    string Status,
    DateTime RequestedAt,
    DateTime? CompletedAt,
    DateTime? FailedAt,
    string FailureReason,
    DateTime CreatedAt,
    DateTime UpdatedAt);
