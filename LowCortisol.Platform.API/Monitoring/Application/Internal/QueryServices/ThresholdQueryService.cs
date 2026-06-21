using LowCortisol.Platform.API.Monitoring.Application.Internal.Parsing;
using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ValueObjects;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Application.Internal.QueryServices;

public sealed class ThresholdQueryService : IThresholdQueryService
{
    private readonly IThresholdRepository _thresholdRepository;

    public ThresholdQueryService(IThresholdRepository thresholdRepository)
    {
        _thresholdRepository = thresholdRepository;
    }

    public Task<IReadOnlyCollection<Threshold>> Handle(
        GetActiveThresholdsQuery query,
        CancellationToken cancellationToken = default) =>
        _thresholdRepository.ListActiveAsync(cancellationToken);

    public Task<IReadOnlyCollection<Threshold>> Handle(
        GetThresholdsByScopeQuery query,
        CancellationToken cancellationToken = default)
    {
        ResourceType? resourceType = null;
        if (!string.IsNullOrWhiteSpace(query.ResourceType)
            && MonitoringEnumParser.TryParse<ResourceType>(query.ResourceType, out var parsedResourceType))
        {
            resourceType = parsedResourceType;
        }

        return _thresholdRepository.ListByScopeAsync(
            query.SiteId,
            query.RoomId,
            query.DeviceGroupId,
            query.SensorId,
            resourceType,
            cancellationToken);
    }
}
