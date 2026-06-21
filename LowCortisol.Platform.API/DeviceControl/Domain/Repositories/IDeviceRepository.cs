using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Repositories;

public interface IDeviceRepository : IBaseRepository<Device>
{
    Task<IReadOnlyCollection<Device>> FindByDeviceGroupIdsAsync(
        IReadOnlyCollection<Guid> deviceGroupIds,
        CancellationToken cancellationToken = default);
}
