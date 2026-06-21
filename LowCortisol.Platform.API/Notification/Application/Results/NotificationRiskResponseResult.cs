using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Entities;

namespace LowCortisol.Platform.API.Notification.Application.Results;

public record NotificationRiskResponseResult(
    Alert Alert,
    Incident Incident,
    AlertDelivery Delivery);
