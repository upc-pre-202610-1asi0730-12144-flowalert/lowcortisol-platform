using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.InMemory.Repositories;

public sealed class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context) : base(context.Rooms)
    {
        _context = context;
    }

    public Task<bool> ExistsByNameInSiteAsync(Guid siteId, string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_context.Rooms.Any(room =>
            room.SiteId == siteId && string.Equals(room.Name, name, StringComparison.OrdinalIgnoreCase)));
    }

    public Task<IReadOnlyCollection<Room>> FindBySiteIdAsync(Guid siteId, CancellationToken cancellationToken = default)
    {
        var rooms = _context.Rooms.Where(room => room.SiteId == siteId).ToList();
        foreach (var room in rooms)
        {
            foreach (var group in _context.DeviceGroups.Where(group => group.RoomId == room.Id))
            {
                room.AttachDeviceGroup(group);
            }
        }

        return Task.FromResult<IReadOnlyCollection<Room>>(rooms);
    }
}
