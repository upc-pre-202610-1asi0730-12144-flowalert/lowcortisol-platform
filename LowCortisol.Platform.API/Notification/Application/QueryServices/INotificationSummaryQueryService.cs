using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Domain.Model.ReadModels;

namespace LowCortisol.Platform.API.Notification.Application.QueryServices;

public interface INotificationSummaryQueryService
{
    Task<NotificationSummary> Handle(GetNotificationSummaryQuery query, CancellationToken cancellationToken = default);
}
