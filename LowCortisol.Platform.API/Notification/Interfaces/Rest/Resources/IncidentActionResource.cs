namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record IncidentActionResource(
    Guid Id,
    Guid IncidentId,
    string ActionType,
    string Description,
    string PerformedBy,
    DateTime PerformedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);
