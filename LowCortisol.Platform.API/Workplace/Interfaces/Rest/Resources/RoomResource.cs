namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

public record RoomResource(
    Guid Id,
    Guid SiteId,
    string Name,
    string Type,
    string Status,
    IReadOnlyCollection<DeviceGroupResource> DeviceGroups);
