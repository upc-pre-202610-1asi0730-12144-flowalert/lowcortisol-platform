using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.InMemory.Repositories;

public sealed class IncidentRepository : BaseRepository<Incident>, IIncidentRepository
{
    private readonly List<Incident> _incidents;

    public IncidentRepository(AppDbContext context) : base(context.Incidents)
    {
        _incidents = context.Incidents;
    }

    public new Task<Incident?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_incidents.FirstOrDefault(incident => incident.Id == id));

    public Task<IReadOnlyCollection<Incident>> ListOpenAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Incident>>(
            _incidents.Where(incident => incident.Status != IncidentStatus.Closed && incident.Status != IncidentStatus.Resolved)
                .OrderByDescending(incident => incident.CreatedAt)
                .ToList());

    public Task<IReadOnlyCollection<Incident>> ListByAlertIdAsync(Guid alertId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Incident>>(
            _incidents.Where(incident => incident.AlertId == alertId)
                .OrderByDescending(incident => incident.CreatedAt)
                .ToList());

    public Task<IReadOnlyCollection<Incident>> ListBySiteIdAsync(Guid siteId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Incident>>(
            _incidents.Where(incident => incident.SiteId == siteId)
                .OrderByDescending(incident => incident.CreatedAt)
                .ToList());

    public Task<int> CountOpenAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_incidents.Count(incident =>
            incident.Status != IncidentStatus.Closed && incident.Status != IncidentStatus.Resolved));

    public Task<int> CountResolvedAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_incidents.Count(incident => incident.Status == IncidentStatus.Resolved));
}
