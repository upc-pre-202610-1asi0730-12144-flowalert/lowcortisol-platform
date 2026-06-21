namespace LowCortisol.Platform.API.DeviceControl.Application.Results;

public record DeviceControlMitigationResult(
    Guid IncidentId,
    Guid DeviceId,
    Guid ValveId,
    Guid DeviceCommandId,
    Guid ValveOperationId,
    string CommandStatus,
    string ValveStatus,
    DateTime ExecutedAt);
