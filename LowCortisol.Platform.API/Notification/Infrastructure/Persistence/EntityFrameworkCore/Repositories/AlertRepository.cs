using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    public AlertRepository(AppDbContext context) : base(context)
    {
    }

    public new Task<Alert?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Context.Alerts
            .Include(alert => alert.Deliveries)
            .FirstOrDefaultAsync(alert => alert.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Alert>> ListOpenAsync(CancellationToken cancellationToken = default) =>
        await Context.Alerts
            .Include(alert => alert.Deliveries)
            .Where(alert => alert.Status == AlertStatus.Open || alert.Status == AlertStatus.Acknowledged)
            .OrderByDescending(alert => alert.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Alert>> ListCriticalAsync(CancellationToken cancellationToken = default) =>
        await Context.Alerts
            .Include(alert => alert.Deliveries)
            .Where(alert => alert.Severity == AlertSeverity.Critical)
            .OrderByDescending(alert => alert.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Alert>> ListBySiteIdAsync(Guid siteId, CancellationToken cancellationToken = default) =>
        await Context.Alerts
            .Include(alert => alert.Deliveries)
            .Where(alert => alert.SiteId == siteId)
            .OrderByDescending(alert => alert.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Alert>> ListByAnomalyIdAsync(Guid anomalyId, CancellationToken cancellationToken = default) =>
        await Context.Alerts
            .Include(alert => alert.Deliveries)
            .Where(alert => alert.AnomalyId == anomalyId)
            .OrderByDescending(alert => alert.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        Context.Alerts.CountAsync(cancellationToken);

    public Task<int> CountOpenAsync(CancellationToken cancellationToken = default) =>
        Context.Alerts.CountAsync(
            alert => alert.Status == AlertStatus.Open || alert.Status == AlertStatus.Acknowledged,
            cancellationToken);

    public Task<int> CountCriticalAsync(CancellationToken cancellationToken = default) =>
        Context.Alerts.CountAsync(alert => alert.Severity == AlertSeverity.Critical, cancellationToken);

    public Task<int> CountAcknowledgedAsync(CancellationToken cancellationToken = default) =>
        Context.Alerts.CountAsync(alert => alert.Status == AlertStatus.Acknowledged, cancellationToken);

    public Task<int> CountResolvedAsync(CancellationToken cancellationToken = default) =>
        Context.Alerts.CountAsync(alert => alert.Status == AlertStatus.Resolved, cancellationToken);

    public Task<int> CountPendingDeliveriesAsync(CancellationToken cancellationToken = default) =>
        Context.AlertDeliveries.CountAsync(delivery => delivery.Status == DeliveryStatus.Pending, cancellationToken);
}
