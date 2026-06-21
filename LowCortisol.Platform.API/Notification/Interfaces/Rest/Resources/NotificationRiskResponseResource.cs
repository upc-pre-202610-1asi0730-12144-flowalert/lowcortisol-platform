namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;

public record NotificationRiskResponseResource(
    AlertResource Alert,
    IncidentResource Incident,
    AlertDeliveryResource Delivery);
