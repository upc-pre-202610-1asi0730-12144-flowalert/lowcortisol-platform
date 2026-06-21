using LowCortisol.Platform.API.DeviceControl.Application.Results;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.DeviceControl.Application.CommandServices;

public interface IValveOperationCommandService
{
    Task<Result<DeviceControlMitigationResult>> Handle(
        CloseValveForIncidentCommand command,
        CancellationToken cancellationToken = default);
}
