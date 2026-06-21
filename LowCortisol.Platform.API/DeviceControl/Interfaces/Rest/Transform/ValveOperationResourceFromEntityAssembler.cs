using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;

public static class ValveOperationResourceFromEntityAssembler
{
    public static ValveOperationResource ToResourceFromEntity(ValveOperation entity) =>
        new(
            entity.Id,
            entity.ValveId,
            entity.DeviceId,
            entity.SiteId,
            entity.RoomId,
            entity.DeviceGroupId,
            entity.IncidentId,
            EnumText.ToResourceValue(entity.ResourceType),
            EnumText.ToResourceValue(entity.PreviousStatus),
            EnumText.ToResourceValue(entity.TargetStatus),
            EnumText.ToResourceValue(entity.Reason),
            EnumText.ToResourceValue(entity.Source),
            EnumText.ToResourceValue(entity.Status),
            entity.RequestedAt,
            entity.CompletedAt,
            entity.FailedAt,
            entity.FailureReason,
            entity.CreatedAt,
            entity.UpdatedAt);
}
