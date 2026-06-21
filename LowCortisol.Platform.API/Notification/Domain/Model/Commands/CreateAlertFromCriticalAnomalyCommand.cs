namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record CreateAlertFromCriticalAnomalyCommand(
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
