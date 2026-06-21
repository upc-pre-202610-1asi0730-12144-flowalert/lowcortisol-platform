namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

public record PhysicalSensorResource(
    Guid Id,
    Guid DeviceId,
    string Name,
    string ResourceType,
    string Status,
    decimal? LastReadingValue);
