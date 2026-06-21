using LowCortisol.Platform.API.Monitoring.Domain.Model.Queries;
using LowCortisol.Platform.API.Monitoring.Domain.Model.ReadModels;

namespace LowCortisol.Platform.API.Monitoring.Application.QueryServices;

public interface IMonitoringSummaryQueryService
{
    Task<MonitoringSummary> Handle(
        GetMonitoringSummaryQuery query,
        CancellationToken cancellationToken = default);
}
