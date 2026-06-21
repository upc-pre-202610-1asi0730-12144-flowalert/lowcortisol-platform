using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class DeviceGroupRepository : BaseRepository<DeviceGroup>, IDeviceGroupRepository
{
    public DeviceGroupRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> ExistsByNameInRoomAsync(
        Guid roomId,
        string name,
        CancellationToken cancellationToken = default)
    {
        return Context.DeviceGroups.AnyAsync(
            group => group.RoomId == roomId && group.Name.ToLower() == name.ToLower(),
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeviceGroup>> FindByRoomIdAsync(
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        return await Context.DeviceGroups
            .Where(group => group.RoomId == roomId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
