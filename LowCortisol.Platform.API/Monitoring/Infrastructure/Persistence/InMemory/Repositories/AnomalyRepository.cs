using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.InMemory.Repositories;

public sealed class AnomalyRepository : BaseRepository<Anomaly>, IAnomalyRepository
{
    private readonly AppDbContext _context;

    public AnomalyRepository(AppDbContext context) : base(context.Anomalies)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<Anomaly>> ListOpenAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Anomaly>>(
            _context.Anomalies.Where(anomaly => anomaly.Status == AnomalyStatus.Open).ToList());

    public Task<IReadOnlyCollection<Anomaly>> ListBySiteIdAsync(
        Guid siteId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Anomaly>>(
            _context.Anomalies.Where(anomaly => anomaly.SiteId == siteId).ToList());

    public Task<int> CountOpenAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_context.Anomalies.Count(anomaly => anomaly.Status == AnomalyStatus.Open));

    public Task<int> CountCriticalOpenAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(
            _context.Anomalies.Count(anomaly =>
                anomaly.Status == AnomalyStatus.Open && anomaly.Severity == AnomalySeverity.Critical));
}
