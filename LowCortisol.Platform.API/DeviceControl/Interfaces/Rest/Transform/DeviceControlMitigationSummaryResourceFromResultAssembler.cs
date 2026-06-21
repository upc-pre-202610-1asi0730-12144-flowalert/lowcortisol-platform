using LowCortisol.Platform.API.DeviceControl.Domain.Model.ReadModels;
using LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Transform;

public static class DeviceControlMitigationSummaryResourceFromResultAssembler
{
    public static DeviceControlMitigationSummaryResource ToResourceFromResult(DeviceControlMitigationSummary result) =>
        new(
            result.TotalCommands,
            result.ExecutedCommands,
            result.FailedCommands,
            result.PendingCommands,
            result.TotalValveOperations,
            result.CompletedValveOperations,
            result.FailedValveOperations,
            result.IncidentMitigations,
            result.LastMitigationAt);
}
