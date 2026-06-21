using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class ValveRepository : BaseRepository<Valve>, IValveRepository
{
    public ValveRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Valve>> FindByDeviceIdsAsync(
        IReadOnlyCollection<Guid> deviceIds,
        CancellationToken cancellationToken = default)
    {
        return await Context.Valves
            .Where(valve => deviceIds.Contains(valve.DeviceId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
