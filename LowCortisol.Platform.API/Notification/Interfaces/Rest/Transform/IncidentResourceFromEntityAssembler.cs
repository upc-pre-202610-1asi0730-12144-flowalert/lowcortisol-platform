using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class IncidentResourceFromEntityAssembler
{
    public static IncidentResource ToResourceFromEntity(Incident entity) =>
        new(
            entity.Id,
            entity.AlertId,
            entity.SiteId,
            entity.RoomId,
            entity.DeviceGroupId,
            entity.DeviceId,
            entity.SensorId,
            EnumText.ToResourceValue(entity.Priority),
            EnumText.ToResourceValue(entity.Status),
            entity.Title,
            entity.Description,
            entity.ResolvedAt,
            entity.ClosedAt,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Actions.Select(IncidentActionResourceFromEntityAssembler.ToResourceFromEntity),
            entity.Assignments.Select(IncidentAssignmentResourceFromEntityAssembler.ToResourceFromEntity));
}
