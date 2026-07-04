using LowCortisol.Platform.API.Plan.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Plan.Domain.Model.Entities;
using LowCortisol.Platform.API.Plan.Domain.Model.Queries;
using LowCortisol.Platform.API.Plan.Domain.Model.ReadModels;
using PlanAggregate = LowCortisol.Platform.API.Plan.Domain.Model.Aggregates.Plan;

namespace LowCortisol.Platform.API.Plan.Application.QueryServices;

public interface IPlanQueryService
{
    Task<IReadOnlyCollection<PlanAggregate>> Handle(GetPlansQuery query, CancellationToken cancellationToken = default);
    Task<PlanAggregate?> Handle(GetPlanByIdQuery query, CancellationToken cancellationToken = default);
    Task<Subscription?> Handle(GetActiveSubscriptionQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Payment>> Handle(GetPaymentsQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ServiceRequest>> Handle(GetServiceRequestsQuery query, CancellationToken cancellationToken = default);
    Task<PlanSummary> Handle(GetPlanSummaryQuery query, CancellationToken cancellationToken = default);
}
