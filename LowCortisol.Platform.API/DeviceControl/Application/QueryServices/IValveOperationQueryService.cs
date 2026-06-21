using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;

namespace LowCortisol.Platform.API.DeviceControl.Application.QueryServices;

public interface IValveOperationQueryService
{
    Task<IReadOnlyCollection<ValveOperation>> Handle(
        GetValveOperationsByValveIdQuery query,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ValveOperation>> Handle(
        GetValveOperationsByIncidentIdQuery query,
        CancellationToken cancellationToken = default);
}
