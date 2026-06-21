using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Domain.Repositories;

public interface IConsumptionReadingRepository : IBaseRepository<ConsumptionReading>
{
    Task<IReadOnlyCollection<ConsumptionReading>> ListRecentAsync(
        int count,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConsumptionReading>> ListBySiteIdAsync(
        Guid siteId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConsumptionReading>> ListByRoomIdAsync(
        Guid roomId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConsumptionReading>> ListByDeviceGroupIdAsync(
        Guid deviceGroupId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConsumptionReading>> ListBySensorIdAsync(
        Guid sensorId,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<decimal> SumByResourceAsync(ResourceType resourceType, CancellationToken cancellationToken = default);
    Task<DateTime?> GetLatestCapturedAtAsync(CancellationToken cancellationToken = default);
    Task<int> CountDistinctSensorsAsync(CancellationToken cancellationToken = default);
}
