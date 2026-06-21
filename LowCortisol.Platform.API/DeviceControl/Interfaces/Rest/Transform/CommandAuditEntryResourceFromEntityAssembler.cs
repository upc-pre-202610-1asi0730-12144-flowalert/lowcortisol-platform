using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;

public static class CommandAuditEntryResourceFromEntityAssembler
{
    public static CommandAuditEntryResource ToResourceFromEntity(CommandAuditEntry entity) =>
        new(
            entity.Id,
            entity.DeviceCommandId,
            entity.Action,
            entity.Description,
            entity.PerformedBy,
            entity.PerformedAt,
            entity.CreatedAt,
            entity.UpdatedAt);
}
