using LowCortisol.Platform.API.DeviceControl.Application.Acl;
using LowCortisol.Platform.API.DeviceControl.Application.CommandServices;
using LowCortisol.Platform.API.DeviceControl.Application.Results;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Commands;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.DeviceControl.Application.Internal.Acl;

public sealed class DeviceControlContextFacade : IDeviceControlContextFacade
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorRepository _sensorRepository;
    private readonly IValveRepository _valveRepository;
    private readonly IValveOperationCommandService _valveOperationCommandService;

    public DeviceControlContextFacade(
        IDeviceRepository deviceRepository,
        ISensorRepository sensorRepository,
        IValveRepository valveRepository,
        IValveOperationCommandService valveOperationCommandService)
    {
        _deviceRepository = deviceRepository;
        _sensorRepository = sensorRepository;
        _valveRepository = valveRepository;
        _valveOperationCommandService = valveOperationCommandService;
    }

    public async Task<IReadOnlyCollection<DeviceGroupInventory>> GetInventoryByDeviceGroupIdsAsync(
        IReadOnlyCollection<Guid> deviceGroupIds,
        CancellationToken cancellationToken = default)
    {
        if (deviceGroupIds.Count == 0) return [];

        var devices = await _deviceRepository.FindByDeviceGroupIdsAsync(deviceGroupIds, cancellationToken);
        var deviceIds = devices.Select(device => device.Id).ToList();
        var sensors = await _sensorRepository.FindByDeviceIdsAsync(deviceIds, cancellationToken);
        var valves = await _valveRepository.FindByDeviceIdsAsync(deviceIds, cancellationToken);

        return deviceGroupIds.Select(deviceGroupId =>
        {
            var groupDevices = devices.Where(device => device.DeviceGroupId == deviceGroupId).ToList();
            var groupDeviceIds = groupDevices.Select(device => device.Id).ToHashSet();

            return new DeviceGroupInventory(
                deviceGroupId,
                groupDevices,
            sensors.Where(sensor => groupDeviceIds.Contains(sensor.DeviceId)).ToList(),
            valves.Where(valve => groupDeviceIds.Contains(valve.DeviceId)).ToList());
        }).ToList();
    }

    public Task<Result<DeviceControlMitigationResult>> CloseValveForIncidentAsync(
        Guid incidentId,
        Guid siteId,
        Guid roomId,
        Guid deviceGroupId,
        Guid deviceId,
        Guid valveId,
        string requestedBy,
        string reason,
        CancellationToken cancellationToken = default)
    {
        return _valveOperationCommandService.Handle(
            new CloseValveForIncidentCommand(
                incidentId,
                siteId,
                roomId,
                deviceGroupId,
                deviceId,
                valveId,
                requestedBy,
                reason),
            cancellationToken);
    }
}
