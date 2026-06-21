namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

public record PhysicalDeviceResource(
    Guid Id,
    string Name,
    string SerialNumber,
    string Type,
    string Status,
    Guid? DeviceGroupId);
