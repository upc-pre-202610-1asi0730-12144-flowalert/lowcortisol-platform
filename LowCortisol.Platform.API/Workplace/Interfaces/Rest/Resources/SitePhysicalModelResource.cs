namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

public record SitePhysicalModelResource(SiteResource Site, IReadOnlyCollection<RoomResource> Rooms);
