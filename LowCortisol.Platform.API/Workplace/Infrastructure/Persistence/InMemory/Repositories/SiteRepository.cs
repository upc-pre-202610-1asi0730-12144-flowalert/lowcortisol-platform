using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.InMemory.Repositories;

public sealed class SiteRepository : BaseRepository<Site>, ISiteRepository
{
    private readonly AppDbContext _context;

    public SiteRepository(AppDbContext context) : base(context.Sites)
    {
        _context = context;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_context.Sites.Any(site =>
            string.Equals(site.Name, name, StringComparison.OrdinalIgnoreCase)));
    }

    public Task<Site?> GetPhysicalModelByIdAsync(Guid siteId, CancellationToken cancellationToken = default)
    {
        var site = _context.Sites.FirstOrDefault(entry => entry.Id == siteId);
        if (site is null) return Task.FromResult<Site?>(null);

        foreach (var room in _context.Rooms.Where(room => room.SiteId == site.Id))
        {
            foreach (var group in _context.DeviceGroups.Where(group => group.RoomId == room.Id))
            {
                room.AttachDeviceGroup(group);
            }

            site.AttachRoom(room);
        }

        return Task.FromResult<Site?>(site);
    }
}
