namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

public record PhysicalValveResource(
    Guid Id,
    Guid DeviceId,
    string Name,
    string ResourceType,
    string Status);
