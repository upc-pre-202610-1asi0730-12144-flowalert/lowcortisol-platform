using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;
using LowCortisol.Platform.API.Notification.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Application.Internal.QueryServices;

public sealed class AlertQueryService : IAlertQueryService
{
    private readonly IAlertRepository _alertRepository;

    public AlertQueryService(IAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }

    public Task<IReadOnlyCollection<Alert>> Handle(GetOpenAlertsQuery query, CancellationToken cancellationToken = default) =>
        _alertRepository.ListOpenAsync(cancellationToken);

    public Task<IReadOnlyCollection<Alert>> Handle(GetCriticalAlertsQuery query, CancellationToken cancellationToken = default) =>
        _alertRepository.ListCriticalAsync(cancellationToken);

    public Task<IReadOnlyCollection<Alert>> Handle(GetAlertsBySiteIdQuery query, CancellationToken cancellationToken = default) =>
        _alertRepository.ListBySiteIdAsync(query.SiteId, cancellationToken);

    public Task<IReadOnlyCollection<Alert>> Handle(GetAlertsByAnomalyIdQuery query, CancellationToken cancellationToken = default) =>
        _alertRepository.ListByAnomalyIdAsync(query.AnomalyId, cancellationToken);

    public Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default) =>
        _alertRepository.FindByIdAsync(query.AlertId, cancellationToken);
}
