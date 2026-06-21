using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Domain.Repositories;

public interface IIncidentRepository : IBaseRepository<Incident>
{
    new Task<Incident?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Incident>> ListOpenAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Incident>> ListByAlertIdAsync(Guid alertId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Incident>> ListBySiteIdAsync(Guid siteId, CancellationToken cancellationToken = default);
    Task<int> CountOpenAsync(CancellationToken cancellationToken = default);
    Task<int> CountResolvedAsync(CancellationToken cancellationToken = default);
}
