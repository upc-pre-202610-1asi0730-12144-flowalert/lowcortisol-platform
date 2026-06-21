using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class IncidentAssignmentResourceFromEntityAssembler
{
    public static IncidentAssignmentResource ToResourceFromEntity(IncidentAssignment entity) =>
        new(
            entity.Id,
            entity.IncidentId,
            entity.AssigneeId,
            entity.AssigneeName,
            entity.AssignedAt,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt);
}
