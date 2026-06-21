using LowCortisol.Platform.API.DeviceControl.Domain.Model.Entities;

namespace LowCortisol.Platform.API.DeviceControl.Application.Acl;

public record DeviceGroupInventory(
    Guid DeviceGroupId,
    IReadOnlyCollection<Device> Devices,
    IReadOnlyCollection<Sensor> Sensors,
    IReadOnlyCollection<Valve> Valves);
