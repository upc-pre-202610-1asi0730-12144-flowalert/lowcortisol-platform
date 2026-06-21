using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class CreateAlertFromCriticalAnomalyCommandFromResourceAssembler
{
    public static CreateAlertFromCriticalAnomalyCommand ToCommandFromResource(
        CreateAlertFromCriticalAnomalyResource resource) =>
        new(
            resource.AnomalyId,
            resource.ReadingId,
            resource.SiteId,
            resource.RoomId,
            resource.DeviceGroupId,
            resource.DeviceId,
            resource.SensorId,
            resource.ResourceType,
            resource.Value,
            resource.LimitValue,
            resource.Unit,
            resource.DetectedAt);
}
