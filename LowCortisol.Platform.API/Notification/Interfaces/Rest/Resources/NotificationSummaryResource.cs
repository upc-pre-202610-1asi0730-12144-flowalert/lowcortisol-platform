namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record NotificationSummaryResource(
    int TotalAlerts,
    int OpenAlerts,
    int CriticalAlerts,
    int AcknowledgedAlerts,
    int ResolvedAlerts,
    int OpenIncidents,
    int ResolvedIncidents,
    int ActiveChannels,
    int PendingDeliveries);
