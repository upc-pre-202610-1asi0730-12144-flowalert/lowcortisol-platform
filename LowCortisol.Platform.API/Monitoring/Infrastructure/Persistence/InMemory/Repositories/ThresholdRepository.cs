using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory;

namespace LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.InMemory.Repositories;

public sealed class ThresholdRepository : BaseRepository<Threshold>, IThresholdRepository
{
    private readonly AppDbContext _context;

    public ThresholdRepository(AppDbContext context) : base(context.Thresholds)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<Threshold>> ListActiveAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Threshold>>(
            _context.Thresholds.Where(threshold => threshold.IsActive).ToList());

    public Task<IReadOnlyCollection<Threshold>> ListApplicableToReadingAsync(
        ConsumptionReading reading,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Threshold>>(
            _context.Thresholds.Where(threshold => threshold.AppliesTo(reading)).ToList());

    public Task<IReadOnlyCollection<Threshold>> ListByScopeAsync(
        Guid? siteId,
        Guid? roomId,
        Guid? deviceGroupId,
        Guid? sensorId,
        ResourceType? resourceType,
        CancellationToken cancellationToken = default)
    {
        var thresholds = _context.Thresholds.Where(threshold =>
            (!siteId.HasValue || threshold.SiteId == siteId)
            && (!roomId.HasValue || threshold.RoomId == roomId)
            && (!deviceGroupId.HasValue || threshold.DeviceGroupId == deviceGroupId)
            && (!sensorId.HasValue || threshold.SensorId == sensorId)
            && (!resourceType.HasValue || threshold.ResourceType == resourceType.Value));

        return Task.FromResult<IReadOnlyCollection<Threshold>>(thresholds.ToList());
    }

    public Task<int> CountActiveAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_context.Thresholds.Count(threshold => threshold.IsActive));
}
