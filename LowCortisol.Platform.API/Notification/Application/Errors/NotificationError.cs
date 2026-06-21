namespace LowCortisol.Platform.API.Notification.Application.Errors;

public enum NotificationError
{
    AlertNotFound,
    IncidentNotFound,
    ChannelNotFound,
    AlertIdRequired,
    IncidentIdRequired,
    ChannelIdRequired,
    TitleRequired,
    SeverityRequired,
    SourceTypeRequired,
    AnomalyIdRequired,
    ResourceTypeRequired,
    InvalidSeverity,
    InvalidStatus,
    InvalidChannelType,
    InvalidPriority,
    InvalidStateTransition,
    DeliveryChannelUnavailable,
    UnexpectedError
}
