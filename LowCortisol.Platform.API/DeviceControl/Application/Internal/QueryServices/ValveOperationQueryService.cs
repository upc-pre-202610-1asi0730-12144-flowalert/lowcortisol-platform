using LowCortisol.Platform.API.DeviceControl.Application.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.DeviceControl.Domain.Model.Queries;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Application.Internal.QueryServices;

public sealed class ValveOperationQueryService : IValveOperationQueryService
{
    private readonly IValveOperationRepository _valveOperationRepository;

    public ValveOperationQueryService(IValveOperationRepository valveOperationRepository)
    {
        _valveOperationRepository = valveOperationRepository;
    }

    public Task<IReadOnlyCollection<ValveOperation>> Handle(
        GetValveOperationsByValveIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return _valveOperationRepository.ListByValveIdAsync(query.ValveId, cancellationToken);
    }

    public Task<IReadOnlyCollection<ValveOperation>> Handle(
        GetValveOperationsByIncidentIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return _valveOperationRepository.ListByIncidentIdAsync(query.IncidentId, cancellationToken);
    }
}
