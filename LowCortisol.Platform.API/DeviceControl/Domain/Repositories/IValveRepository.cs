using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Repositories;

public interface IValveRepository : IBaseRepository<Valve>
{
    Task<IReadOnlyCollection<Valve>> FindByDeviceIdsAsync(
        IReadOnlyCollection<Guid> deviceIds,
        CancellationToken cancellationToken = default);
}
