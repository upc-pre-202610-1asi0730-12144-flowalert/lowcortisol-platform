using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;

public static class DeviceCommandResourceFromEntityAssembler
{
    public static DeviceCommandResource ToResourceFromEntity(DeviceCommand entity) =>
        new(
            entity.Id,
            entity.DeviceId,
            entity.ValveId,
            entity.SiteId,
            entity.RoomId,
            entity.DeviceGroupId,
            entity.IncidentId,
            EnumText.ToResourceValue(entity.CommandType),
            EnumText.ToResourceValue(entity.Status),
            EnumText.ToResourceValue(entity.Source),
            entity.Reason,
            entity.RequestedBy,
            entity.RequestedAt,
            entity.ExecutedAt,
            entity.FailedAt,
            entity.FailureReason,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.Executions.Select(CommandExecutionResourceFromEntityAssembler.ToResourceFromEntity),
            entity.AuditEntries.Select(CommandAuditEntryResourceFromEntityAssembler.ToResourceFromEntity));
}
