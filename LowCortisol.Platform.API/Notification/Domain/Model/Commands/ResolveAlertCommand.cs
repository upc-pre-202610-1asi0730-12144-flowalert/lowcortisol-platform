namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record ResolveAlertCommand(Guid AlertId, string ResolvedBy, string Note);
