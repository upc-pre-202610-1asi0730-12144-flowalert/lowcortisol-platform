namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record CreateNotificationChannelResource(string Name, string Type, bool IsActive = true);
