using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;

namespace LowCortisol.Platform.API.DeviceControl.Application.QueryServices;

public interface IDeviceCommandQueryService
{
    Task<IReadOnlyCollection<DeviceCommand>> Handle(
        GetDeviceCommandsQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<DeviceCommand>> Handle(
        GetDeviceCommandsByDeviceIdQuery query,
        CancellationToken cancellationToken = default);
}
