namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record NotificationChannelResource(
    Guid Id,
    string Name,
    string Type,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
