using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    public RoomRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> ExistsByNameInSiteAsync(
        Guid siteId,
        string name,
        CancellationToken cancellationToken = default)
    {
        return Context.Rooms.AnyAsync(
            room => room.SiteId == siteId && room.Name.ToLower() == name.ToLower(),
            cancellationToken);
    }

    public async Task<IReadOnlyCollection<Room>> FindBySiteIdAsync(
        Guid siteId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Rooms
            .Include(room => room.DeviceGroups)
            .Where(room => room.SiteId == siteId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
