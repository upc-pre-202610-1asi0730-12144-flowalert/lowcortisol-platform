namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

public record ConsumptionReadingResource(
    Guid Id,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid DeviceId,
    Guid SensorId,
    string ResourceType,
    decimal Value,
    string Unit,
    DateTime CapturedAt,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);
