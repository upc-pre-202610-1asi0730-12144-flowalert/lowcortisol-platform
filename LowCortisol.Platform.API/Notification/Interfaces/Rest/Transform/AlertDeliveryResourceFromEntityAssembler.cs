using LowCortisol.Platform.API.Notification.Domain.Model.Entities;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class AlertDeliveryResourceFromEntityAssembler
{
    public static AlertDeliveryResource ToResourceFromEntity(AlertDelivery entity) =>
        new(
            entity.Id,
            entity.AlertId,
            entity.ChannelId,
            EnumText.ToResourceValue(entity.ChannelType),
            EnumText.ToResourceValue(entity.Status),
            entity.Recipient.UserId,
            entity.Recipient.Email,
            entity.Recipient.DisplayName,
            entity.Message.Title,
            entity.Message.Description,
            entity.AttemptedAt,
            entity.SentAt,
            entity.FailureReason,
            entity.CreatedAt,
            entity.UpdatedAt);
}
