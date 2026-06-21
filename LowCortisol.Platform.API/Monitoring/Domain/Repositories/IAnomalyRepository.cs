using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Domain.Repositories;

public interface IAnomalyRepository : IBaseRepository<Anomaly>
{
    Task<IReadOnlyCollection<Anomaly>> ListOpenAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Anomaly>> ListBySiteIdAsync(
        Guid siteId,
        CancellationToken cancellationToken = default);

    Task<int> CountOpenAsync(CancellationToken cancellationToken = default);
    Task<int> CountCriticalOpenAsync(CancellationToken cancellationToken = default);
}
