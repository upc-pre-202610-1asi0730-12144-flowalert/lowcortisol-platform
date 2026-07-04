namespace LowCortisol.Platform.API.Plan.Interfaces.Rest.Resources;

public record PlanFeatureResource(Guid Id, Guid PlanId, string Name, string Description, DateTime CreatedAt, DateTime UpdatedAt);

public record PlanResource(
    Guid Id,
    string Code,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string BillingPeriod,
    int MaxSites,
    int MaxDevices,
    bool IsRecommended,
    bool IsActive,
    IReadOnlyCollection<PlanFeatureResource> Features,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record SubscriptionResource(
    Guid Id,
    string UserId,
    string WorkplaceId,
    Guid PlanId,
    string Status,
    DateTime StartedAt,
    DateTime? ExpiresAt,
    bool AutoRenew,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record PaymentResource(
    Guid Id,
    Guid SubscriptionId,
    decimal Amount,
    string Currency,
    string Method,
    string Status,
    DateTime? PaidAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record ServiceRequestResource(
    Guid Id,
    Guid SubscriptionId,
    string Type,
    string Description,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record PlanSummaryResource(
    int TotalPlans,
    string ActivePlanName,
    string SubscriptionStatus,
    int TotalPayments,
    decimal TotalPaid,
    int ServiceRequest,
    int MaxSites,
    int MaxDevices,
    int UsedSites,
    int UsedDevices,
    int RemainingSites,
    int RemainingDevices);

public record SubscribeToPlanResource(string UserId, string WorkplaceId, Guid PlanId, string PaymentMethod);
public record ChangePlanResource(string UserId, Guid NewPlanId);
public record CancelSubscriptionResource(string UserId, string Reason);
