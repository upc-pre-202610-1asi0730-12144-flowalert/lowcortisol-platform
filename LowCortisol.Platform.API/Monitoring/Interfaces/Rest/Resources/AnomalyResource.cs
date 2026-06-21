namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

public record AnomalyResource(
    Guid Id,
    Guid ReadingId,
    Guid ThresholdId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid DeviceId,
    Guid SensorId,
    string ResourceType,
    decimal Value,
    decimal LimitValue,
    string Unit,
    string Severity,
    string Status,
    string Description,
    DateTime DetectedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);
