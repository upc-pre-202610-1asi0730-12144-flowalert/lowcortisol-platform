namespace LowCortisol.Platform.API.Notification.Domain.Model.Commands;

public record ResolveIncidentCommand(Guid IncidentId, string ResolvedBy, string Resolution);
