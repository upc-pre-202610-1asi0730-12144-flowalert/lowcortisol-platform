namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record AcknowledgeAlertCommand(Guid AlertId, string AcknowledgedBy);
