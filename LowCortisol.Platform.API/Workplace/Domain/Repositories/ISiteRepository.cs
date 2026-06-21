using LowCortisol.Platform.API.Shared.Domain.Repositories;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;

namespace LowCortisol.Platform.API.Workplace.Domain.Repositories;

public interface ISiteRepository : IBaseRepository<Site>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Site?> GetPhysicalModelByIdAsync(Guid siteId, CancellationToken cancellationToken = default);
}
