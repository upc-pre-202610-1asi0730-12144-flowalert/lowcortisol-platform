namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

public record CreateThresholdResource(
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
    bool? IsActive);
