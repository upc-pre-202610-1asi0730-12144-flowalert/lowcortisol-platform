using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories;

public sealed class ValveRepository : BaseRepository<Valve>, IValveRepository
{
    private readonly AppDbContext _context;

    public ValveRepository(AppDbContext context) : base(context.Valves)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<Valve>> FindByDeviceIdsAsync(
        IReadOnlyCollection<Guid> deviceIds,
        CancellationToken cancellationToken = default)
    {
        var ids = deviceIds.ToHashSet();

        return Task.FromResult<IReadOnlyCollection<Valve>>(
            _context.Valves.Where(valve => ids.Contains(valve.DeviceId)).ToList());
    }
}
