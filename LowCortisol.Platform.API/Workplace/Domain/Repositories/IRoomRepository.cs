using LowCortisol.Platform.API.Shared.Domain.Repositories;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;

namespace LowCortisol.Platform.API.Workplace.Domain.Repositories;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<bool> ExistsByNameInSiteAsync(Guid siteId, string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Room>> FindBySiteIdAsync(Guid siteId, CancellationToken cancellationToken = default);
}
