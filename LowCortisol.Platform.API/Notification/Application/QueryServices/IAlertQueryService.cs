using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Queries;

namespace LowCortisol.Platform.API.Notification.Application.QueryServices;

public interface IAlertQueryService
{
    Task<IReadOnlyCollection<Alert>> Handle(GetOpenAlertsQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Alert>> Handle(GetCriticalAlertsQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Alert>> Handle(GetAlertsBySiteIdQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Alert>> Handle(GetAlertsByAnomalyIdQuery query, CancellationToken cancellationToken = default);
    Task<Alert?> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default);
}
