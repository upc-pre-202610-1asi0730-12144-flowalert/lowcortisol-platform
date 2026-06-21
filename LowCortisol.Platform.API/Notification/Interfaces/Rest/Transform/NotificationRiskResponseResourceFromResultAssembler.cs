using LowCortisol.Platform.API.Notification.Application.Results;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class NotificationRiskResponseResourceFromResultAssembler
{
    public static NotificationRiskResponseResource ToResourceFromResult(NotificationRiskResponseResult result) =>
        new(
            AlertResourceFromEntityAssembler.ToResourceFromEntity(result.Alert),
            IncidentResourceFromEntityAssembler.ToResourceFromEntity(result.Incident),
            AlertDeliveryResourceFromEntityAssembler.ToResourceFromEntity(result.Delivery));
}
