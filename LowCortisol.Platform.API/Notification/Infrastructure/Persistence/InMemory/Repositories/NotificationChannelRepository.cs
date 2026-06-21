using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.InMemory.Repositories;

public sealed class NotificationChannelRepository : BaseRepository<NotificationChannel>, INotificationChannelRepository
{
    private readonly List<NotificationChannel> _channels;

    public NotificationChannelRepository(AppDbContext context) : base(context.NotificationChannels)
    {
        _channels = context.NotificationChannels;
    }

    public new Task<NotificationChannel?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_channels.FirstOrDefault(channel => channel.Id == id));

    public Task<IReadOnlyCollection<NotificationChannel>> ListActiveAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<NotificationChannel>>(
            _channels.Where(channel => channel.IsActive).OrderBy(channel => channel.Name).ToList());

    public Task<NotificationChannel?> FindDefaultInAppAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_channels.FirstOrDefault(channel => channel.Type == NotificationChannelType.InApp));

    public Task<int> CountActiveAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_channels.Count(channel => channel.IsActive));
}
