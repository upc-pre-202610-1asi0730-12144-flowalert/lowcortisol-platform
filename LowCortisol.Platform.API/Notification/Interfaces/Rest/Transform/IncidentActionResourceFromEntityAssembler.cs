using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class IncidentActionResourceFromEntityAssembler
{
    public static IncidentActionResource ToResourceFromEntity(IncidentAction entity) =>
        new(
            entity.Id,
            entity.IncidentId,
            entity.ActionType,
            entity.Description,
            entity.PerformedBy,
            entity.PerformedAt,
            entity.CreatedAt,
            entity.UpdatedAt);
}
