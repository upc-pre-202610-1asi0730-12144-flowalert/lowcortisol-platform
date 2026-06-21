using LowCortisol.Platform.API.DeviceControl.Application.Acl;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;
using LowCortisol.Platform.API.Workplace.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Workplace.Domain.Model.Entities;
using LowCortisol.Platform.API.Workplace.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Workplace.Interfaces.Rest.Transform;

public static class SitePhysicalModelResourceFromEntityAssembler
{
    public static SitePhysicalModelResource ToResourceFromEntity(
        Site site,
        IReadOnlyCollection<DeviceGroupInventory> inventories)
    {
        var inventoryByGroupId = inventories.ToDictionary(inventory => inventory.DeviceGroupId);

        return new SitePhysicalModelResource(
            SiteResourceFromEntityAssembler.ToResourceFromEntity(site),
            site.Rooms.Select(room => ToRoomResource(room, inventoryByGroupId)).ToList());
    }

    private static RoomResource ToRoomResource(
        Room room,
        IReadOnlyDictionary<Guid, DeviceGroupInventory> inventoryByGroupId) =>
        new(
            room.Id,
            room.SiteId,
            room.Name,
            EnumText.ToResourceValue(room.Type),
            EnumText.ToResourceValue(room.Status),
            room.DeviceGroups.Select(group => ToDeviceGroupResource(group, inventoryByGroupId)).ToList());

    private static DeviceGroupResource ToDeviceGroupResource(
        DeviceGroup group,
        IReadOnlyDictionary<Guid, DeviceGroupInventory> inventoryByGroupId)
    {
        inventoryByGroupId.TryGetValue(group.Id, out var inventory);

        return new DeviceGroupResource(
            group.Id,
            group.RoomId,
            group.Name,
            EnumText.ToResourceValue(group.ResourceType),
            EnumText.ToResourceValue(group.Status),
            inventory?.Devices.Select(device => new PhysicalDeviceResource(
                device.Id,
                device.Name,
                device.SerialNumber,
                EnumText.ToResourceValue(device.Type),
                EnumText.ToResourceValue(device.Status),
                device.DeviceGroupId)).ToList() ?? [],
            inventory?.Sensors.Select(sensor => new PhysicalSensorResource(
                sensor.Id,
                sensor.DeviceId,
                sensor.Name,
                EnumText.ToResourceValue(sensor.ResourceType),
                EnumText.ToResourceValue(sensor.Status),
                sensor.LastReadingValue)).ToList() ?? [],
            inventory?.Valves.Select(valve => new PhysicalValveResource(
                valve.Id,
                valve.DeviceId,
                valve.Name,
                EnumText.ToResourceValue(valve.ResourceType),
                EnumText.ToResourceValue(valve.Status))).ToList() ?? []);
    }
}
