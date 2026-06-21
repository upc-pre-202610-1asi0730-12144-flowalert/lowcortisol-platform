namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record IncidentResource(
    Guid Id,
    Guid AlertId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid DeviceId,
    Guid SensorId,
    string Priority,
    string Status,
    string Title,
    string Description,
    DateTime? ResolvedAt,
    DateTime? ClosedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<IncidentActionResource> Actions,
    IEnumerable<IncidentAssignmentResource> Assignments);
