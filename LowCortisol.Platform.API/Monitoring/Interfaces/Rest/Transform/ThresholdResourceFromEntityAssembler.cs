using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;

public static class ThresholdResourceFromEntityAssembler
{
    public static ThresholdResource ToResourceFromEntity(Threshold entity) =>
        new(
            entity.Id,
            entity.SiteId,
            entity.RoomId,
            entity.DeviceGroupId,
            entity.SensorId,
            EnumText.ToResourceValue(entity.ResourceType),
            entity.Name,
            EnumText.ToResourceValue(entity.Operator),
            entity.LimitValue,
            entity.Unit,
            EnumText.ToResourceValue(entity.Severity),
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt);
}
