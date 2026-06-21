using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;

public static class SiteResourceFromEntityAssembler
{
    public static SiteResource ToResourceFromEntity(Site entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.Address,
            EnumText.ToResourceValue(entity.Type),
            EnumText.ToResourceValue(entity.Status),
            entity.Rooms.Count,
            entity.Rooms.Sum(room => room.DeviceGroups.Count));
}
