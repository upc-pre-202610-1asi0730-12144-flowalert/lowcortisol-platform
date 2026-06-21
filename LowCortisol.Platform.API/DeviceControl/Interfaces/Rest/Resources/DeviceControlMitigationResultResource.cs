namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

public record DeviceControlMitigationResultResource(
    Guid IncidentId,
    Guid DeviceId,
    Guid ValveId,
    Guid DeviceCommandId,
    Guid ValveOperationId,
    string CommandStatus,
    string ValveStatus,
    DateTime ExecutedAt);
