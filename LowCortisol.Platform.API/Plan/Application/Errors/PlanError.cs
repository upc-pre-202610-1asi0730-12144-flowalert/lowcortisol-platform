namespace LowCortisol.Platform.API.Plan.Application.Errors;

public enum PlanError
{
    PlanNotFound,
    SubscriptionNotFound,
    ServiceRequestNotFound,
    UserIdRequired,
    PlanDoesNotCoverUsage,
    UnexpectedError
}
