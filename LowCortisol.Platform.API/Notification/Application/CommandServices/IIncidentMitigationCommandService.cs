using LowCortisol.Platform.API.Notification.Application.Results;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Notification.Application.CommandServices;

public interface IIncidentMitigationCommandService
{
    Task<Result<IncidentMitigationResult>> Handle(
        RequestIncidentMitigationCommand command,
        CancellationToken cancellationToken = default);
}
