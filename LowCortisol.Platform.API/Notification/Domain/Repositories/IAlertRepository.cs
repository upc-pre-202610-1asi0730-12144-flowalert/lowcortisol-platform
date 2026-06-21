using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Domain.Repositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    new Task<Alert?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Alert>> ListOpenAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Alert>> ListCriticalAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Alert>> ListBySiteIdAsync(Guid siteId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Alert>> ListByAnomalyIdAsync(Guid anomalyId, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountOpenAsync(CancellationToken cancellationToken = default);
    Task<int> CountCriticalAsync(CancellationToken cancellationToken = default);
    Task<int> CountAcknowledgedAsync(CancellationToken cancellationToken = default);
    Task<int> CountResolvedAsync(CancellationToken cancellationToken = default);
    Task<int> CountPendingDeliveriesAsync(CancellationToken cancellationToken = default);
}
