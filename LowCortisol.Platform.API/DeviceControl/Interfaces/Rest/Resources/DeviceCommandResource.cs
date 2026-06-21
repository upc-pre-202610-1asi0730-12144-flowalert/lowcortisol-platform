namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

public record DeviceCommandResource(
    Guid Id,
    Guid DeviceId,
    Guid? ValveId,
    Guid SiteId,
    Guid RoomId,
    Guid DeviceGroupId,
    Guid? IncidentId,
    string CommandType,
    string Status,
    string Source,
    string Reason,
    string RequestedBy,
    DateTime RequestedAt,
    DateTime? ExecutedAt,
    DateTime? FailedAt,
    string FailureReason,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<CommandExecutionResource> Executions,
    IEnumerable<CommandAuditEntryResource> AuditEntries);
