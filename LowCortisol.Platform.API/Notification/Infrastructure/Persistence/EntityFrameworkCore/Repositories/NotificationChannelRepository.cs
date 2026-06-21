using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class NotificationChannelRepository : BaseRepository<NotificationChannel>, INotificationChannelRepository
{
    public NotificationChannelRepository(AppDbContext context) : base(context)
    {
    }

    public new Task<NotificationChannel?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Context.NotificationChannels.FirstOrDefaultAsync(channel => channel.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<NotificationChannel>> ListActiveAsync(CancellationToken cancellationToken = default) =>
        await Context.NotificationChannels
            .Where(channel => channel.IsActive)
            .OrderBy(channel => channel.Name)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<NotificationChannel?> FindDefaultInAppAsync(CancellationToken cancellationToken = default) =>
        Context.NotificationChannels
            .FirstOrDefaultAsync(channel => channel.Type == NotificationChannelType.InApp, cancellationToken);

    public Task<int> CountActiveAsync(CancellationToken cancellationToken = default) =>
        Context.NotificationChannels.CountAsync(channel => channel.IsActive, cancellationToken);
}
