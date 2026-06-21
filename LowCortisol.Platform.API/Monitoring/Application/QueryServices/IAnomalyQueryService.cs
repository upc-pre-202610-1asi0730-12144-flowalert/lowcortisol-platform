using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;

namespace LowCortisol.Platform.API.Monitoring.Application.QueryServices;

public interface IAnomalyQueryService
{
    Task<IReadOnlyCollection<Anomaly>> Handle(
        GetOpenAnomaliesQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Anomaly>> Handle(
        GetAnomaliesBySiteIdQuery query,
        CancellationToken cancellationToken = default);
}
