using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.InMemory.Repositories;

public sealed class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    private readonly List<Alert> _alerts;

    public AlertRepository(AppDbContext context) : base(context.Alerts)
    {
        _alerts = context.Alerts;
    }

    public new Task<Alert?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_alerts.FirstOrDefault(alert => alert.Id == id));

    public Task<IReadOnlyCollection<Alert>> ListOpenAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Alert>>(
            _alerts.Where(alert => alert.Status is AlertStatus.Open or AlertStatus.Acknowledged)
                .OrderByDescending(alert => alert.CreatedAt)
                .ToList());

    public Task<IReadOnlyCollection<Alert>> ListCriticalAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Alert>>(
            _alerts.Where(alert => alert.Severity == AlertSeverity.Critical)
                .OrderByDescending(alert => alert.CreatedAt)
                .ToList());

    public Task<IReadOnlyCollection<Alert>> ListBySiteIdAsync(Guid siteId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Alert>>(
            _alerts.Where(alert => alert.SiteId == siteId)
                .OrderByDescending(alert => alert.CreatedAt)
                .ToList());

    public Task<IReadOnlyCollection<Alert>> ListByAnomalyIdAsync(Guid anomalyId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Alert>>(
            _alerts.Where(alert => alert.AnomalyId == anomalyId)
                .OrderByDescending(alert => alert.CreatedAt)
                .ToList());

    public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_alerts.Count);

    public Task<int> CountOpenAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_alerts.Count(alert => alert.Status is AlertStatus.Open or AlertStatus.Acknowledged));

    public Task<int> CountCriticalAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_alerts.Count(alert => alert.Severity == AlertSeverity.Critical));

    public Task<int> CountAcknowledgedAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_alerts.Count(alert => alert.Status == AlertStatus.Acknowledged));

    public Task<int> CountResolvedAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_alerts.Count(alert => alert.Status == AlertStatus.Resolved));

    public Task<int> CountPendingDeliveriesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_alerts.SelectMany(alert => alert.Deliveries)
            .Count(delivery => delivery.Status == DeliveryStatus.Pending));
}
