namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

public record SiteResource(
    Guid Id,
    string Name,
    string Address,
    string Type,
    string Status,
    int RoomCount,
    int DeviceGroupCount);
