namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record AssignIncidentCommand(Guid IncidentId, string AssigneeId, string AssigneeName);
