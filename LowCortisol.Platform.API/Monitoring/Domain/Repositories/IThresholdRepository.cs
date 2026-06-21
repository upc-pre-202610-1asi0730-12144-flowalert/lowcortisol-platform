using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Domain.Repositories;

public interface IThresholdRepository : IBaseRepository<Threshold>
{
    Task<IReadOnlyCollection<Threshold>> ListActiveAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Threshold>> ListApplicableToReadingAsync(
        ConsumptionReading reading,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Threshold>> ListByScopeAsync(
        Guid? siteId,
        Guid? roomId,
        Guid? deviceGroupId,
        Guid? sensorId,
        ResourceType? resourceType,
        CancellationToken cancellationToken = default);

    Task<int> CountActiveAsync(CancellationToken cancellationToken = default);
}
