using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert entity) =>
        new(
            entity.Id,
            entity.Message.Title,
            entity.Message.Description,
            EnumText.ToResourceValue(entity.Severity),
            EnumText.ToResourceValue(entity.Status),
            EnumText.ToResourceValue(entity.SourceType),
            entity.AnomalyId,
            entity.ReadingId,
            entity.SiteId,
            entity.RoomId,
            entity.DeviceGroupId,
            entity.DeviceId,
            entity.SensorId,
            entity.ResourceType,
            entity.ResponseSla.MinutesToAcknowledge,
            entity.ResponseSla.MinutesToResolve,
            entity.AcknowledgedAt,
            entity.ResolvedAt,
            entity.ClosedAt,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Deliveries.Select(AlertDeliveryResourceFromEntityAssembler.ToResourceFromEntity));
}
