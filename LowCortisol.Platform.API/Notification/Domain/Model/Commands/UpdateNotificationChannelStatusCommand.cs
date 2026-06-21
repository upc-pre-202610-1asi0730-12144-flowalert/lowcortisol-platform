namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record UpdateNotificationChannelStatusCommand(Guid ChannelId, bool IsActive);
