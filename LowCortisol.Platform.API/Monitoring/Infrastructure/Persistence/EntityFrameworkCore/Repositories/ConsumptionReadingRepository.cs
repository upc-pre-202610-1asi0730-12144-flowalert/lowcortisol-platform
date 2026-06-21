using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class ConsumptionReadingRepository : BaseRepository<ConsumptionReading>, IConsumptionReadingRepository
{
    public ConsumptionReadingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<ConsumptionReading>> ListRecentAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        return await Context.ConsumptionReadings
            .OrderByDescending(reading => reading.CapturedAt)
            .Take(count)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ConsumptionReading>> ListBySiteIdAsync(
        Guid siteId,
        CancellationToken cancellationToken = default)
    {
        return await Context.ConsumptionReadings
            .Where(reading => reading.SiteId == siteId)
            .OrderByDescending(reading => reading.CapturedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ConsumptionReading>> ListByRoomIdAsync(
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        return await Context.ConsumptionReadings
            .Where(reading => reading.RoomId == roomId)
            .OrderByDescending(reading => reading.CapturedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ConsumptionReading>> ListByDeviceGroupIdAsync(
        Guid deviceGroupId,
        CancellationToken cancellationToken = default)
    {
        return await Context.ConsumptionReadings
            .Where(reading => reading.DeviceGroupId == deviceGroupId)
            .OrderByDescending(reading => reading.CapturedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ConsumptionReading>> ListBySensorIdAsync(
        Guid sensorId,
        CancellationToken cancellationToken = default)
    {
        return await Context.ConsumptionReadings
            .Where(reading => reading.SensorId == sensorId)
            .OrderByDescending(reading => reading.CapturedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        Context.ConsumptionReadings.CountAsync(cancellationToken);

    public Task<decimal> SumByResourceAsync(
        ResourceType resourceType,
        CancellationToken cancellationToken = default) =>
        Context.ConsumptionReadings
            .Where(reading => reading.ResourceType == resourceType)
            .SumAsync(reading => reading.Value, cancellationToken);

    public Task<DateTime?> GetLatestCapturedAtAsync(CancellationToken cancellationToken = default) =>
        Context.ConsumptionReadings
            .OrderByDescending(reading => reading.CapturedAt)
            .Select(reading => (DateTime?)reading.CapturedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<int> CountDistinctSensorsAsync(CancellationToken cancellationToken = default) =>
        Context.ConsumptionReadings
            .Select(reading => reading.SensorId)
            .Distinct()
            .CountAsync(cancellationToken);
}
