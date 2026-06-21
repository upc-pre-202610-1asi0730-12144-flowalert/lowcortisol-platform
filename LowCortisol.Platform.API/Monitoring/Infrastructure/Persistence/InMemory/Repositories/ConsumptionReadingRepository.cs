using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.InMemory.Repositories;

public sealed class ConsumptionReadingRepository : BaseRepository<ConsumptionReading>, IConsumptionReadingRepository
{
    private readonly AppDbContext _context;

    public ConsumptionReadingRepository(AppDbContext context) : base(context.ConsumptionReadings)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<ConsumptionReading>> ListRecentAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        var readings = _context.ConsumptionReadings
            .OrderByDescending(reading => reading.CapturedAt)
            .Take(count)
            .ToList();

        return Task.FromResult<IReadOnlyCollection<ConsumptionReading>>(readings);
    }

    public Task<IReadOnlyCollection<ConsumptionReading>> ListBySiteIdAsync(
        Guid siteId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<ConsumptionReading>>(
            _context.ConsumptionReadings.Where(reading => reading.SiteId == siteId).ToList());

    public Task<IReadOnlyCollection<ConsumptionReading>> ListByRoomIdAsync(
        Guid roomId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<ConsumptionReading>>(
            _context.ConsumptionReadings.Where(reading => reading.RoomId == roomId).ToList());

    public Task<IReadOnlyCollection<ConsumptionReading>> ListByDeviceGroupIdAsync(
        Guid deviceGroupId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<ConsumptionReading>>(
            _context.ConsumptionReadings.Where(reading => reading.DeviceGroupId == deviceGroupId).ToList());

    public Task<IReadOnlyCollection<ConsumptionReading>> ListBySensorIdAsync(
        Guid sensorId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<ConsumptionReading>>(
            _context.ConsumptionReadings.Where(reading => reading.SensorId == sensorId).ToList());

    public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_context.ConsumptionReadings.Count);

    public Task<decimal> SumByResourceAsync(
        ResourceType resourceType,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(
            _context.ConsumptionReadings
                .Where(reading => reading.ResourceType == resourceType)
                .Sum(reading => reading.Value));

    public Task<DateTime?> GetLatestCapturedAtAsync(CancellationToken cancellationToken = default)
    {
        var latest = _context.ConsumptionReadings
            .OrderByDescending(reading => reading.CapturedAt)
            .Select(reading => (DateTime?)reading.CapturedAt)
            .FirstOrDefault();

        return Task.FromResult(latest);
    }

    public Task<int> CountDistinctSensorsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_context.ConsumptionReadings.Select(reading => reading.SensorId).Distinct().Count());
}
