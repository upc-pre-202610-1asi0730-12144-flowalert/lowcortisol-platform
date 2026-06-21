using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;

public static class CommandExecutionResourceFromEntityAssembler
{
    public static CommandExecutionResource ToResourceFromEntity(CommandExecution entity) =>
        new(
            entity.Id,
            entity.DeviceCommandId,
            EnumText.ToResourceValue(entity.Status),
            entity.StartedAt,
            entity.FinishedAt,
            entity.ResultMessage,
            entity.FailureReason,
            entity.CreatedAt,
            entity.UpdatedAt);
}
