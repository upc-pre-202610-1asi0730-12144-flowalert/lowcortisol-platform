namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record AlertDeliveryResource(
    Guid Id,
    Guid AlertId,
    Guid ChannelId,
    string ChannelType,
    string Status,
    string RecipientUserId,
    string RecipientEmail,
    string RecipientDisplayName,
    string MessageTitle,
    string MessageDescription,
    DateTime AttemptedAt,
    DateTime? SentAt,
    string FailureReason,
    DateTime CreatedAt,
    DateTime UpdatedAt);
