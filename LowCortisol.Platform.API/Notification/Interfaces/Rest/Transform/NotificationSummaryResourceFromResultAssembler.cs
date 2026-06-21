using LowCortisol.Platform.API.Notification.Domain.Model.ReadModels;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class NotificationSummaryResourceFromResultAssembler
{
    public static NotificationSummaryResource ToResourceFromResult(NotificationSummary summary) =>
        new(
            summary.TotalAlerts,
            summary.OpenAlerts,
            summary.CriticalAlerts,
            summary.AcknowledgedAlerts,
            summary.ResolvedAlerts,
            summary.OpenIncidents,
            summary.ResolvedIncidents,
            summary.ActiveChannels,
            summary.PendingDeliveries);
}
