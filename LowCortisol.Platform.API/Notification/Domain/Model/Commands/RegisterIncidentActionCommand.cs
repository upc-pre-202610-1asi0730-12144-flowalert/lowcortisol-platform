namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record RegisterIncidentActionCommand(Guid IncidentId, string ActionType, string Description, string PerformedBy);
