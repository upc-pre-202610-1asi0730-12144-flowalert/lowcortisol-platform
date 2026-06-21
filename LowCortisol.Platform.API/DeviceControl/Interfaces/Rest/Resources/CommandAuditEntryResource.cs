namespace LowCortisol.Platform.API.DeviceControl.Interfaces.Rest.Resources;

public record CommandAuditEntryResource(
    Guid Id,
    Guid DeviceCommandId,
    string Action,
    string Description,
    string PerformedBy,
    DateTime PerformedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);
