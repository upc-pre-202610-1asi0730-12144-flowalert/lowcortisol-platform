namespace LowCortisol.Platform.API.DeviceControl.Domain.Model.ReadModels;

public record DeviceControlMitigationSummary(
    int TotalCommands,
    int ExecutedCommands,
    int FailedCommands,
    int PendingCommands,
    int TotalValveOperations,
    int CompletedValveOperations,
    int FailedValveOperations,
    int IncidentMitigations,
    DateTime? LastMitigationAt);
