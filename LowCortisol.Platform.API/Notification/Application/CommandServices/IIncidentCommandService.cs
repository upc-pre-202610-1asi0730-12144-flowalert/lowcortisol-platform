using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Notification.Application.CommandServices;

public interface IIncidentCommandService
{
    Task<Result<Incident>> Handle(CreateIncidentFromAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<Incident>> Handle(AssignIncidentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Incident>> Handle(RegisterIncidentActionCommand command, CancellationToken cancellationToken = default);
    Task<Result<Incident>> Handle(ResolveIncidentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Incident>> Handle(CloseIncidentCommand command, CancellationToken cancellationToken = default);
}
