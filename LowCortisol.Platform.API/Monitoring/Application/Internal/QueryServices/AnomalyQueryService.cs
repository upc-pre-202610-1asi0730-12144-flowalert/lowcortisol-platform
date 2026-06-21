using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;

namespace LowCortisol.Platform.API.Monitoring.Application.Internal.QueryServices;

public sealed class AnomalyQueryService : IAnomalyQueryService
{
    private readonly IAnomalyRepository _anomalyRepository;

    public AnomalyQueryService(IAnomalyRepository anomalyRepository)
    {
        _anomalyRepository = anomalyRepository;
    }

    public Task<IReadOnlyCollection<Anomaly>> Handle(
        GetOpenAnomaliesQuery query,
        CancellationToken cancellationToken = default) =>
        _anomalyRepository.ListOpenAsync(cancellationToken);

    public Task<IReadOnlyCollection<Anomaly>> Handle(
        GetAnomaliesBySiteIdQuery query,
        CancellationToken cancellationToken = default) =>
        _anomalyRepository.ListBySiteIdAsync(query.SiteId, cancellationToken);
}
