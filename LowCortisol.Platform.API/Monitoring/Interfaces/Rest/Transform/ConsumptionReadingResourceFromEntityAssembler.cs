using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;

public static class ConsumptionReadingResourceFromEntityAssembler
{
    public static ConsumptionReadingResource ToResourceFromEntity(ConsumptionReading entity) =>
        new(
            entity.Id,
            entity.SiteId,
            entity.RoomId,
            entity.DeviceGroupId,
            entity.DeviceId,
            entity.SensorId,
            EnumText.ToResourceValue(entity.ResourceType),
            entity.Value,
            entity.Unit,
            entity.CapturedAt,
            EnumText.ToResourceValue(entity.Status),
            entity.CreatedAt,
            entity.UpdatedAt);
}
