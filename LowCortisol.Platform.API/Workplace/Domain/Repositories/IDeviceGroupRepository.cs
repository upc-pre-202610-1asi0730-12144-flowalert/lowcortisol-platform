using LowCortisol.Platform.API.Shared.Domain.Repositories;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;

namespace LowCortisol.Platform.API.Workplace.Domain.Repositories;

public interface IDeviceGroupRepository : IBaseRepository<DeviceGroup>
{
    Task<bool> ExistsByNameInRoomAsync(Guid roomId, string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<DeviceGroup>> FindByRoomIdAsync(Guid roomId, CancellationToken cancellationToken = default);
}
