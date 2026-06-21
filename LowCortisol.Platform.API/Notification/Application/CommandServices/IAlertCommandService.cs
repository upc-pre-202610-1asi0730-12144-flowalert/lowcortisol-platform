using LowCortisol.Platform.API.Notification.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Notification.Domain.Model.Commands;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Notification.Application.CommandServices;

public interface IAlertCommandService
{
    Task<Result<Alert>> Handle(CreateAlertFromCriticalAnomalyCommand command, CancellationToken cancellationToken = default);
    Task<Result<Alert>> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<Alert>> Handle(CloseAlertCommand command, CancellationToken cancellationToken = default);
}
