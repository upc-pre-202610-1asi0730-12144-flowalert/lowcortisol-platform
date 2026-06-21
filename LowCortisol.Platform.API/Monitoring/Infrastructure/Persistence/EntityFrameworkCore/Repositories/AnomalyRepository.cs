using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class AnomalyRepository : BaseRepository<Anomaly>, IAnomalyRepository
{
    public AnomalyRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Anomaly>> ListOpenAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Anomalies
            .Where(anomaly => anomaly.Status == AnomalyStatus.Open)
            .OrderByDescending(anomaly => anomaly.DetectedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Anomaly>> ListBySiteIdAsync(
        Guid siteId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Anomalies
            .Where(anomaly => anomaly.SiteId == siteId)
            .OrderByDescending(anomaly => anomaly.DetectedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountOpenAsync(CancellationToken cancellationToken = default) =>
        Context.Anomalies.CountAsync(anomaly => anomaly.Status == AnomalyStatus.Open, cancellationToken);

    public Task<int> CountCriticalOpenAsync(CancellationToken cancellationToken = default) =>
        Context.Anomalies.CountAsync(
            anomaly => anomaly.Status == AnomalyStatus.Open && anomaly.Severity == AnomalySeverity.Critical,
            cancellationToken);
}
