using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories;

public sealed class DeviceRepository : BaseRepository<Device>, IDeviceRepository
{
    private readonly AppDbContext _context;

    public DeviceRepository(AppDbContext context) : base(context.Devices)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<Device>> FindByDeviceGroupIdsAsync(
        IReadOnlyCollection<Guid> deviceGroupIds,
        CancellationToken cancellationToken = default)
    {
        var ids = deviceGroupIds.ToHashSet();

        return Task.FromResult<IReadOnlyCollection<Device>>(
            _context.Devices
                .Where(device => device.DeviceGroupId.HasValue && ids.Contains(device.DeviceGroupId.Value))
                .ToList());
    }
}
