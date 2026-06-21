using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class IncidentRepository : BaseRepository<Incident>, IIncidentRepository
{
    public IncidentRepository(AppDbContext context) : base(context)
    {
    }

    public new Task<Incident?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Context.Incidents
            .Include(incident => incident.Actions)
            .Include(incident => incident.Assignments)
            .FirstOrDefaultAsync(incident => incident.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Incident>> ListOpenAsync(CancellationToken cancellationToken = default) =>
        await Context.Incidents
            .Include(incident => incident.Actions)
            .Include(incident => incident.Assignments)
            .Where(incident => incident.Status != IncidentStatus.Closed && incident.Status != IncidentStatus.Resolved)
            .OrderByDescending(incident => incident.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Incident>> ListByAlertIdAsync(Guid alertId, CancellationToken cancellationToken = default) =>
        await Context.Incidents
            .Include(incident => incident.Actions)
            .Include(incident => incident.Assignments)
            .Where(incident => incident.AlertId == alertId)
            .OrderByDescending(incident => incident.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Incident>> ListBySiteIdAsync(Guid siteId, CancellationToken cancellationToken = default) =>
        await Context.Incidents
            .Include(incident => incident.Actions)
            .Include(incident => incident.Assignments)
            .Where(incident => incident.SiteId == siteId)
            .OrderByDescending(incident => incident.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<int> CountOpenAsync(CancellationToken cancellationToken = default) =>
        Context.Incidents.CountAsync(
            incident => incident.Status != IncidentStatus.Closed && incident.Status != IncidentStatus.Resolved,
            cancellationToken);

    public Task<int> CountResolvedAsync(CancellationToken cancellationToken = default) =>
        Context.Incidents.CountAsync(incident => incident.Status == IncidentStatus.Resolved, cancellationToken);
}
