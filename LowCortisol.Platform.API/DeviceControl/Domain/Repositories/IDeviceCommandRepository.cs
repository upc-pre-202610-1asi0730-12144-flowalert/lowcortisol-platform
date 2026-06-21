using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Repositories;

public interface IDeviceCommandRepository : IBaseRepository<DeviceCommand>
{
    Task<IReadOnlyCollection<DeviceCommand>> ListRecentAsync(int count, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<DeviceCommand>> ListByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<DeviceCommand>> ListByIncidentIdAsync(Guid incidentId, CancellationToken cancellationToken = default);
}
