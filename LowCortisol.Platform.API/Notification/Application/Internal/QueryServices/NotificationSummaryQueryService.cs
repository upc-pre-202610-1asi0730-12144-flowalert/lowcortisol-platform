using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Domain.Model.ReadModels;
using LowCortisol.Platform.API.Notification.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.QueryServices;

public sealed class NotificationSummaryQueryService : INotificationSummaryQueryService
{
    private readonly IAlertRepository _alertRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly INotificationChannelRepository _channelRepository;

    public NotificationSummaryQueryService(
        IAlertRepository alertRepository,
        IIncidentRepository incidentRepository,
        INotificationChannelRepository channelRepository)
    {
        _alertRepository = alertRepository;
        _incidentRepository = incidentRepository;
        _channelRepository = channelRepository;
    }

    public async Task<NotificationSummary> Handle(
        GetNotificationSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        return new NotificationSummary(
            await _alertRepository.CountAsync(cancellationToken),
            await _alertRepository.CountOpenAsync(cancellationToken),
            await _alertRepository.CountCriticalAsync(cancellationToken),
            await _alertRepository.CountAcknowledgedAsync(cancellationToken),
            await _alertRepository.CountResolvedAsync(cancellationToken),
            await _incidentRepository.CountOpenAsync(cancellationToken),
            await _incidentRepository.CountResolvedAsync(cancellationToken),
            await _channelRepository.CountActiveAsync(cancellationToken),
            await _alertRepository.CountPendingDeliveriesAsync(cancellationToken));
    }
}
