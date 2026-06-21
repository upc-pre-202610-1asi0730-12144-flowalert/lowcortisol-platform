using LowCortisol.Platform.API.DeviceControl.Application.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Application.Internal.QueryServices;

public sealed class DeviceCommandQueryService : IDeviceCommandQueryService
{
    private readonly IDeviceCommandRepository _deviceCommandRepository;

    public DeviceCommandQueryService(IDeviceCommandRepository deviceCommandRepository)
    {
        _deviceCommandRepository = deviceCommandRepository;
    }

    public Task<IReadOnlyCollection<DeviceCommand>> Handle(
        GetDeviceCommandsQuery query,
        CancellationToken cancellationToken = default)
    {
        var count = query.Count <= 0 ? 20 : query.Count;
        return _deviceCommandRepository.ListRecentAsync(count, cancellationToken);
    }

    public Task<IReadOnlyCollection<DeviceCommand>> Handle(
        GetDeviceCommandsByDeviceIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return _deviceCommandRepository.ListByDeviceIdAsync(query.DeviceId, cancellationToken);
    }
}
