using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Commands;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Shared.Application.Results;

namespace LowCortisol.Platform.API.Plan.Application.CommandServices;

public interface IPlanCommandService
{
    Task<Result<Subscription>> Handle(SubscribeToPlanCommand command, CancellationToken cancellationToken = default);
    Task<Result<Subscription>> Handle(ChangePlanCommand command, CancellationToken cancellationToken = default);
    Task<Result<Subscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken = default);
    Task<Result<ServiceRequest>> Handle(ResolveServiceRequestCommand command, CancellationToken cancellationToken = default);
    Task<Result<ServiceRequest>> Handle(CloseServiceRequestCommand command, CancellationToken cancellationToken = default);
}
