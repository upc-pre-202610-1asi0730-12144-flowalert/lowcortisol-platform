namespace LowCortisol.Platform.API.Notification.Domain.Model.ReadModels;

public record NotificationSummary(
    int TotalAlerts,
    int OpenAlerts,
    int CriticalAlerts,
    int AcknowledgedAlerts,
    int ResolvedAlerts,
    int OpenIncidents,
    int ResolvedIncidents,
    int ActiveChannels,
    int PendingDeliveries);
