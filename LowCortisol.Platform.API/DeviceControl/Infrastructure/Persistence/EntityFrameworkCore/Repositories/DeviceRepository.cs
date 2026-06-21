using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class DeviceRepository : BaseRepository<Device>, IDeviceRepository
{
    public DeviceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Device>> FindByDeviceGroupIdsAsync(
        IReadOnlyCollection<Guid> deviceGroupIds,
        CancellationToken cancellationToken = default)
    {
        return await Context.Devices
            .Where(device => device.DeviceGroupId.HasValue && deviceGroupIds.Contains(device.DeviceGroupId.Value))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
