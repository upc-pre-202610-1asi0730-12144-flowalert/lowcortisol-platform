namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record CreateAlertFromCriticalAnomalyResource(
    Guid AnomalyId,
    Guid ReadingId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid DeviceId,
    Guid SensorId,
    string ResourceType,
    decimal Value,
    decimal LimitValue,
    string Unit,
    DateTime DetectedAt);
