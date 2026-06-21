using LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;

public static class RegisterConsumptionReadingCommandFromResourceAssembler
{
    public static RegisterConsumptionReadingCommand ToCommandFromResource(
        RegisterConsumptionReadingResource resource) =>
        new(
            resource.SiteId,
            resource.RoomId,
            resource.DeviceGroupId,
            resource.DeviceId,
            resource.SensorId,
            resource.ResourceType,
            resource.Value,
            resource.Unit,
            resource.CapturedAt);
}
