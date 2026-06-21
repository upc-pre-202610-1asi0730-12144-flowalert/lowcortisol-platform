using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Repositories;

public interface ISensorRepository : IBaseRepository<Sensor>
{
    Task<IReadOnlyCollection<Sensor>> FindByDeviceIdsAsync(
        IReadOnlyCollection<Guid> deviceIds,
        CancellationToken cancellationToken = default);
}
