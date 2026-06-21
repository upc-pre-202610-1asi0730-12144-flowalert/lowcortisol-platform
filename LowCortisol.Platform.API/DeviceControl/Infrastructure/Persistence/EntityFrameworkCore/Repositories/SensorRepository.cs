using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class SensorRepository : BaseRepository<Sensor>, ISensorRepository
{
    public SensorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Sensor>> FindByDeviceIdsAsync(
        IReadOnlyCollection<Guid> deviceIds,
        CancellationToken cancellationToken = default)
    {
        return await Context.Sensors
            .Where(sensor => deviceIds.Contains(sensor.DeviceId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
