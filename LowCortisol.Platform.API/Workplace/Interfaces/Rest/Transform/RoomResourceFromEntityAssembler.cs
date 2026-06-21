using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;

public static class RoomResourceFromEntityAssembler
{
    public static RoomResource ToResourceFromEntity(Room entity) =>
        new(
            entity.Id,
            entity.SiteId,
            entity.Name,
            EnumText.ToResourceValue(entity.Type),
            EnumText.ToResourceValue(entity.Status),
            entity.DeviceGroups.Select(DeviceGroupResourceFromEntityAssembler.ToResourceFromEntity).ToList());
}
