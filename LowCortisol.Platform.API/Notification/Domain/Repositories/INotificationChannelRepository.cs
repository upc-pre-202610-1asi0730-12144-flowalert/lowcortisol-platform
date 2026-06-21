using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Notification.Domain.Repositories;

public interface INotificationChannelRepository : IBaseRepository<NotificationChannel>
{
    new Task<NotificationChannel?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<NotificationChannel>> ListActiveAsync(CancellationToken cancellationToken = default);
    Task<NotificationChannel?> FindDefaultInAppAsync(CancellationToken cancellationToken = default);
    Task<int> CountActiveAsync(CancellationToken cancellationToken = default);
}
