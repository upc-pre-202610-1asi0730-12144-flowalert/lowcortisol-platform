using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories;

public sealed class ValveOperationRepository : BaseRepository<ValveOperation>, IValveOperationRepository
{
    private readonly AppDbContext _context;

    public ValveOperationRepository(AppDbContext context) : base(context.ValveOperations)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<ValveOperation>> ListRecentAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<ValveOperation>>(
            _context.ValveOperations
                .OrderByDescending(operation => operation.RequestedAt)
                .Take(count)
                .ToList());
    }

    public Task<IReadOnlyCollection<ValveOperation>> ListByValveIdAsync(
        Guid valveId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<ValveOperation>>(
            _context.ValveOperations
                .Where(operation => operation.ValveId == valveId)
                .OrderByDescending(operation => operation.RequestedAt)
                .ToList());
    }

    public Task<IReadOnlyCollection<ValveOperation>> ListByIncidentIdAsync(
        Guid incidentId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<ValveOperation>>(
            _context.ValveOperations
                .Where(operation => operation.IncidentId == incidentId)
                .OrderByDescending(operation => operation.RequestedAt)
                .ToList());
    }
}
