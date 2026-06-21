namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record IncidentAssignmentResource(
    Guid Id,
    Guid IncidentId,
    string AssigneeId,
    string AssigneeName,
    DateTime AssignedAt,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
