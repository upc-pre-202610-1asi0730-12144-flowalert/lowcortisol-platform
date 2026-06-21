using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;

public static class AnomalyResourceFromEntityAssembler
{
    public static AnomalyResource ToResourceFromEntity(Anomaly entity) =>
        new(
            entity.Id,
            entity.ReadingId,
            entity.ThresholdId,
            entity.SiteId,
            entity.RoomId,
            entity.DeviceGroupId,
            entity.DeviceId,
            entity.SensorId,
            EnumText.ToResourceValue(entity.ResourceType),
            entity.Value,
            entity.LimitValue,
            entity.Unit,
            EnumText.ToResourceValue(entity.Severity),
            EnumText.ToResourceValue(entity.Status),
            entity.Description,
            entity.DetectedAt,
            entity.CreatedAt,
            entity.UpdatedAt);
}
