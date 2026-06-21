namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record CloseIncidentCommand(Guid IncidentId, string ClosedBy, string ClosingNote);
