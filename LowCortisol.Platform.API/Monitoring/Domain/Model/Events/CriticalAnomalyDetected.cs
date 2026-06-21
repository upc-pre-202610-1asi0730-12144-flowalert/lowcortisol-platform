using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;

namespace LowCortisol.Platform.API.Monitoring.Domain.Model.Events;

public record CriticalAnomalyDetected(
    Guid AnomalyId,
    Guid ReadingId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid DeviceId,
    Guid SensorId,
    ResourceType ResourceType,
    decimal Value,
    decimal LimitValue,
    string Unit,
    DateTime DetectedAt);
