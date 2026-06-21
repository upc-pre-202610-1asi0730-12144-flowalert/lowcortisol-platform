namespace LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;

public record CreateThresholdCommand(
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
    bool IsActive);
