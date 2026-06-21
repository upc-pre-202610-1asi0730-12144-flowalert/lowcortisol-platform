namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

public record DeviceGroupResource(
    Guid Id,
    Guid RoomId,
    string Name,
    string ResourceType,
    string Status,
    IReadOnlyCollection<PhysicalDeviceResource> Devices,
    IReadOnlyCollection<PhysicalSensorResource> Sensors,
    IReadOnlyCollection<PhysicalValveResource> Valves);
