using LowCortisol.Platform.API.Monitoring.Domain.Model.Events;
using LowCortisol.Platform.API.Notification.Application.Results;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Notification.Application.Acl;

public interface INotificationContextFacade
{
    Task<Result<NotificationRiskResponseResult>> HandleCriticalAnomalyDetectedAsync(
        CriticalAnomalyDetected criticalEvent,
        CancellationToken cancellationToken = default);
}
