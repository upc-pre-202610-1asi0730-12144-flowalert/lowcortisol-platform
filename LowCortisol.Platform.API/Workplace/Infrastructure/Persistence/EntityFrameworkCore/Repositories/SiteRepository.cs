using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class SiteRepository : BaseRepository<Site>, ISiteRepository
{
    public SiteRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Context.Sites.AnyAsync(
            site => site.Name.ToLower() == name.ToLower(),
            cancellationToken);
    }

    public Task<Site?> GetPhysicalModelByIdAsync(Guid siteId, CancellationToken cancellationToken = default)
    {
        return Context.Sites
            .Include(site => site.Rooms)
            .ThenInclude(room => room.DeviceGroups)
            .FirstOrDefaultAsync(site => site.Id == siteId, cancellationToken);
    }
}
