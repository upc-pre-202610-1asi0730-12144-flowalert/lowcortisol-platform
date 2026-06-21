namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

public record CommandExecutionResource(
    Guid Id,
    Guid DeviceCommandId,
    string Status,
    DateTime StartedAt,
    DateTime? FinishedAt,
    string ResultMessage,
    string FailureReason,
    DateTime CreatedAt,
    DateTime UpdatedAt);
