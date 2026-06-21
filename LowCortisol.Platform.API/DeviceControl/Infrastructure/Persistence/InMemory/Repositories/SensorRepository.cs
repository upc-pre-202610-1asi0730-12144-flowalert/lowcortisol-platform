using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories;

public sealed class SensorRepository : BaseRepository<Sensor>, ISensorRepository
{
    private readonly AppDbContext _context;

    public SensorRepository(AppDbContext context) : base(context.Sensors)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<Sensor>> FindByDeviceIdsAsync(
        IReadOnlyCollection<Guid> deviceIds,
        CancellationToken cancellationToken = default)
    {
        var ids = deviceIds.ToHashSet();

        return Task.FromResult<IReadOnlyCollection<Sensor>>(
            _context.Sensors.Where(sensor => ids.Contains(sensor.DeviceId)).ToList());
    }
}
