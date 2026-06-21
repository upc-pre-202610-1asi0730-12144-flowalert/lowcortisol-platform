namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record CloseAlertCommand(Guid AlertId, string ClosedBy, string Note);
