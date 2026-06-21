namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

public record DeviceControlMitigationSummaryResource(
    int TotalCommands,
    int ExecutedCommands,
    int FailedCommands,
    int PendingCommands,
    int TotalValveOperations,
    int CompletedValveOperations,
    int FailedValveOperations,
    int IncidentMitigations,
    DateTime? LastMitigationAt);
