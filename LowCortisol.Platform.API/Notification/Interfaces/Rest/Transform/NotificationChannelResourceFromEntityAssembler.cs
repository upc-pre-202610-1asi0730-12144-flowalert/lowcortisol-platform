using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.Transform;

namespace LowCortisol.Platform.API.Notification.Interfaces.Rest.Transform;

public static class NotificationChannelResourceFromEntityAssembler
{
    public static NotificationChannelResource ToResourceFromEntity(NotificationChannel entity) =>
        new(
            entity.Id,
            entity.Name,
            EnumText.ToResourceValue(entity.Type),
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt);
}
