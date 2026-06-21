using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;

namespace LowCortisol.Platform.API.Notification.Application.QueryServices;

public interface INotificationChannelQueryService
{
    Task<IReadOnlyCollection<NotificationChannel>> Handle(GetNotificationChannelsQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<NotificationChannel>> Handle(GetActiveNotificationChannelsQuery query, CancellationToken cancellationToken = default);
}
