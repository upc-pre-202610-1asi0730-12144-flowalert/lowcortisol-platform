using LowCortisol.Platform.API.DeviceControl.Application.Results;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.DeviceControl.Application.Acl;

public interface IDeviceControlContextFacade
{
    Task<IReadOnlyCollection<DeviceGroupInventory>> GetInventoryByDeviceGroupIdsAsync(
        IReadOnlyCollection<Guid> deviceGroupIds,
        CancellationToken cancellationToken = default);

    Task<Result<DeviceControlMitigationResult>> CloseValveForIncidentAsync(
        Guid incidentId,
        Guid siteId,
        Guid roomId,
        Guid deviceGroupId,
        Guid deviceId,
        Guid valveId,
        string requestedBy,
        string reason,
        CancellationToken cancellationToken = default);
}
