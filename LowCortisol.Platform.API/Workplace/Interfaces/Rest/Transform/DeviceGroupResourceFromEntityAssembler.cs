using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;

public static class DeviceGroupResourceFromEntityAssembler
{
    public static DeviceGroupResource ToResourceFromEntity(DeviceGroup entity) =>
        new(
            entity.Id,
            entity.RoomId,
            entity.Name,
            EnumText.ToResourceValue(entity.ResourceType),
            EnumText.ToResourceValue(entity.Status),
            [],
            [],
            []);
}
