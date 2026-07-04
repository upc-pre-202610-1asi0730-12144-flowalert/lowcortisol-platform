namespace LowCortisol.Platform.API.Plan.Domain.Model.Queries;

public record GetPlansQuery(bool RecommendedOnly = false);
public record GetPlanByIdQuery(Guid PlanId);
public record GetActiveSubscriptionQuery(string UserId);
public record GetPaymentsQuery(string UserId);
public record GetServiceRequestsQuery(string UserId);
public record GetPlanSummaryQuery(string UserId);
