namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record CreateNotificationChannelCommand(string Name, string Type, bool IsActive = true);
