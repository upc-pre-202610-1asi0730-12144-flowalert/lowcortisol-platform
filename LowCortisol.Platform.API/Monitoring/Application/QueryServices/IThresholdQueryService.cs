using LowCortisol.Platform.API.Monitoring.Domain.Model.Entities;
using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;

namespace LowCortisol.Platform.API.Monitoring.Application.QueryServices;

public interface IThresholdQueryService
{
    Task<IReadOnlyCollection<Threshold>> Handle(
        GetActiveThresholdsQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Threshold>> Handle(
        GetThresholdsByScopeQuery query,
        CancellationToken cancellationToken = default);
}
