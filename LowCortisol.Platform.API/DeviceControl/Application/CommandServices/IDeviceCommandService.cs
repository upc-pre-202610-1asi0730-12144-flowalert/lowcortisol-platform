using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.DeviceControl.Application.CommandServices;

public interface IDeviceCommandService
{
    Task<Result<DeviceCommand>> Handle(
        ExecuteDeviceCommandCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<DeviceCommand>> Handle(
        CancelDeviceCommandCommand command,
        CancellationToken cancellationToken = default);
}
