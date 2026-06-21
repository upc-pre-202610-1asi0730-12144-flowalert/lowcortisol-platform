namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

public record ThresholdResource(
    Guid Id,
    Guid? SiteId,
    Guid? RoomId,
    Guid? DeviceGroupId,
    Guid? SensorId,
    string ResourceType,
    string Name,
    string Operator,
    decimal LimitValue,
    string Unit,
    string Severity,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
