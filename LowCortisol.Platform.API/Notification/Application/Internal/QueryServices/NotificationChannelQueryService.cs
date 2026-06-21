using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.QueryServices;

public sealed class NotificationChannelQueryService : INotificationChannelQueryService
{
    private readonly INotificationChannelRepository _channelRepository;

    public NotificationChannelQueryService(INotificationChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public Task<IReadOnlyCollection<NotificationChannel>> Handle(
        GetNotificationChannelsQuery query,
        CancellationToken cancellationToken = default) =>
        _channelRepository.ListAsync(cancellationToken);

    public Task<IReadOnlyCollection<NotificationChannel>> Handle(
        GetActiveNotificationChannelsQuery query,
        CancellationToken cancellationToken = default) =>
        _channelRepository.ListActiveAsync(cancellationToken);
}
