namespace LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;

public record RegisterConsumptionReadingCommand(
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid DeviceId,
    Guid SensorId,
    string ResourceType,
    decimal Value,
    string Unit,
    DateTime? CapturedAt);
