using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.InMemory.Repositories;

public sealed class DeviceGroupRepository : BaseRepository<DeviceGroup>, IDeviceGroupRepository
{
    private readonly AppDbContext _context;

    public DeviceGroupRepository(AppDbContext context) : base(context.DeviceGroups)
    {
        _context = context;
    }

    public Task<bool> ExistsByNameInRoomAsync(Guid roomId, string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_context.DeviceGroups.Any(group =>
            group.RoomId == roomId && string.Equals(group.Name, name, StringComparison.OrdinalIgnoreCase)));
    }

    public Task<IReadOnlyCollection<DeviceGroup>> FindByRoomIdAsync(
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<DeviceGroup>>(
            _context.DeviceGroups.Where(group => group.RoomId == roomId).ToList());
    }
}
