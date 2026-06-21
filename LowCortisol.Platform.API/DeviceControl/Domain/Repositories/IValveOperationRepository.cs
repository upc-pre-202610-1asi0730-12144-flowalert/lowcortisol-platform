using LowCortisol.Platform.API.DeviceControl.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Shared.Domain.Repositories;

namespace LowCortisol.Platform.API.DeviceControl.Domain.Repositories;

public interface IValveOperationRepository : IBaseRepository<ValveOperation>
{
    Task<IReadOnlyCollection<ValveOperation>> ListRecentAsync(int count, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ValveOperation>> ListByValveIdAsync(Guid valveId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ValveOperation>> ListByIncidentIdAsync(Guid incidentId, CancellationToken cancellationToken = default);
}
