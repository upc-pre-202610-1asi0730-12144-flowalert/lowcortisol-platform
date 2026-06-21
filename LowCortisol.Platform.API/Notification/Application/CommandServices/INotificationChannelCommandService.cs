using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Notification.Application.CommandServices;

public interface INotificationChannelCommandService
{
    Task<Result<NotificationChannel>> Handle(CreateNotificationChannelCommand command, CancellationToken cancellationToken = default);
    Task<Result<NotificationChannel>> Handle(UpdateNotificationChannelStatusCommand command, CancellationToken cancellationToken = default);
}
