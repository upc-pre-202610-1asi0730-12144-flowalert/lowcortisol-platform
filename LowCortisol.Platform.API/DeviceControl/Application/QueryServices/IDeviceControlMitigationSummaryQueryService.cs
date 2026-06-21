using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.ReadModels;

namespace LowCortisol.Platform.API.DeviceControl.Application.QueryServices;

public interface IDeviceControlMitigationSummaryQueryService
{
    Task<DeviceControlMitigationSummary> Handle(
        GetDeviceControlMitigationSummaryQuery query,
        CancellationToken cancellationToken = default);
}
