using LowCortisol.Platform.API.Monitoring.Domain.Model.Commands;
using LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Monitoring.Interfaces.Rest.Transform;

public static class CreateThresholdCommandFromResourceAssembler
{
    public static CreateThresholdCommand ToCommandFromResource(CreateThresholdResource resource) =>
        new(
            resource.SiteId,
            resource.RoomId,
            resource.DeviceGroupId,
            resource.SensorId,
            resource.ResourceType,
            resource.Name,
            resource.Operator,
            resource.LimitValue,
            resource.Unit,
            resource.Severity,
            resource.IsActive ?? true);
}
