using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class ThresholdRepository : BaseRepository<Threshold>, IThresholdRepository
{
    public ThresholdRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Threshold>> ListActiveAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Thresholds
            .Where(threshold => threshold.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Threshold>> ListApplicableToReadingAsync(
        ConsumptionReading reading,
        CancellationToken cancellationToken = default)
    {
        return await Context.Thresholds
            .Where(threshold => threshold.IsActive
                && threshold.ResourceType == reading.ResourceType
                && (!threshold.SiteId.HasValue || threshold.SiteId == reading.SiteId)
                && (!threshold.RoomId.HasValue || threshold.RoomId == reading.RoomId)
                && (!threshold.DeviceGroupId.HasValue || threshold.DeviceGroupId == reading.DeviceGroupId)
                && (!threshold.SensorId.HasValue || threshold.SensorId == reading.SensorId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Threshold>> ListByScopeAsync(
        Guid? siteId,
        Guid? roomId,
        Guid? deviceGroupId,
        Guid? sensorId,
        ResourceType? resourceType,
        CancellationToken cancellationToken = default)
    {
        return await Context.Thresholds
            .Where(threshold =>
                (!siteId.HasValue || threshold.SiteId == siteId)
                && (!roomId.HasValue || threshold.RoomId == roomId)
                && (!deviceGroupId.HasValue || threshold.DeviceGroupId == deviceGroupId)
                && (!sensorId.HasValue || threshold.SensorId == sensorId)
                && (!resourceType.HasValue || threshold.ResourceType == resourceType.Value))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountActiveAsync(CancellationToken cancellationToken = default) =>
        Context.Thresholds.CountAsync(threshold => threshold.IsActive, cancellationToken);
}
