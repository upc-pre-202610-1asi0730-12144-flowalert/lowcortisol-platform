namespace LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;

public record GetThresholdsByScopeQuery(
    Guid? SiteId,
    Guid? RoomId,
    Guid? DeviceGroupId,
    Guid? SensorId,
    string? ResourceType = null);
