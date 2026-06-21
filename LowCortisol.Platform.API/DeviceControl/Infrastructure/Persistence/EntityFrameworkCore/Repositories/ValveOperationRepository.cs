using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class ValveOperationRepository : BaseRepository<ValveOperation>, IValveOperationRepository
{
    public ValveOperationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<ValveOperation>> ListRecentAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        return await Context.ValveOperations
            .OrderByDescending(operation => operation.RequestedAt)
            .Take(count)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ValveOperation>> ListByValveIdAsync(
        Guid valveId,
        CancellationToken cancellationToken = default)
    {
        return await Context.ValveOperations
            .Where(operation => operation.ValveId == valveId)
            .OrderByDescending(operation => operation.RequestedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ValveOperation>> ListByIncidentIdAsync(
        Guid incidentId,
        CancellationToken cancellationToken = default)
    {
        return await Context.ValveOperations
            .Where(operation => operation.IncidentId == incidentId)
            .OrderByDescending(operation => operation.RequestedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
